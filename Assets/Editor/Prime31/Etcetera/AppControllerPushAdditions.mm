//
//  AppControllerPushAdditions.m
//  EtceteraTest


#import "AppControllerPushAdditions.h"
#import "EtceteraManager.h"


void UnitySendMessage( const char * className, const char * methodName, const char * param );


@implementation AppControllerPushAdditions

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Class methods

+ (void)load
{
	UnityRegisterAppDelegateListener( [self sharedInstance] );
	[[NSNotificationCenter defaultCenter] addObserver:[self sharedInstance]
											 selector:@selector(applicationDidFinishLaunchingNotification:)
												 name:UIApplicationDidFinishLaunchingNotification object:nil];
}


+ (AppControllerPushAdditions*)sharedInstance
{
	static AppControllerPushAdditions *sharedInstance;
	static dispatch_once_t onceToken;
	dispatch_once( &onceToken, ^{
		sharedInstance = [[self alloc] init];
	});

	return sharedInstance;
}



///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - NSNotification

- (void)applicationDidFinishLaunchingNotification:(NSNotification*)note
{
	if( note.userInfo )
	{
		NSDictionary *remoteNotificationDictionary = [note.userInfo objectForKey:UIApplicationLaunchOptionsRemoteNotificationKey];
		if( remoteNotificationDictionary )
		{
			NSLog( @"launched with remote notification: %@", remoteNotificationDictionary );
			double delayInSeconds = 5.0;
			dispatch_time_t popTime = dispatch_time( DISPATCH_TIME_NOW, (int64_t)(delayInSeconds * NSEC_PER_SEC) );
			dispatch_after( popTime, dispatch_get_main_queue(), ^
			{
				[self handleNotification:remoteNotificationDictionary isLaunchNotification:YES];
			});
		}

		UILocalNotification *localNotification = [note.userInfo objectForKey:UIApplicationLaunchOptionsLocalNotificationKey];
		if( localNotification )
		{
			NSLog( @"launched with local notification: %@", localNotification );
			double delayInSeconds = 5.0;
			dispatch_time_t popTime = dispatch_time( DISPATCH_TIME_NOW, (int64_t)(delayInSeconds * NSEC_PER_SEC) );
			dispatch_after( popTime, dispatch_get_main_queue(), ^
			{
				[self handleLocalNotification:localNotification isLaunchNotification:YES];
			});
		}
	}
}


- (void)didRegisterForRemoteNotificationsWithDeviceToken:(NSNotification*)notification
{
	NSLog( @"didRegisterForRemoteNotificationsWithDeviceToken: %@", notification.userInfo );
	NSData *deviceToken = (NSData*)notification.userInfo;

	NSString *deviceTokenString = [[[[deviceToken description]
									 stringByReplacingOccurrencesOfString:@"<" withString:@""]
									stringByReplacingOccurrencesOfString:@">" withString:@""]
								   stringByReplacingOccurrencesOfString:@" " withString:@""];

	UnitySendMessage( "EtceteraManager", "remoteRegistrationDidSucceed", [deviceTokenString UTF8String] );

	// If this is a user deregistering for notifications, dont proceed past this point
	if( [AppControllerPushAdditions enabledRemoteNotificationTypes] == 0 )
	{
		NSLog( @"Notifications are disabled for this application. Not registering with Urban Airship" );
		return;
	}

	// Grab the Urban Airship info from the info.plist file
	NSString *appKey = [EtceteraManager sharedManager].urbanAirshipAppKey;
	NSString *appSecret = [EtceteraManager sharedManager].urbanAirshipAppSecret;
	NSString *alias = [EtceteraManager sharedManager].urbanAirshipAlias;

	if( !appKey || !appSecret )
		return;

	// Register the deviceToken with Urban Airship
	NSString *UAServer = @"https://go.urbanairship.com";
	NSString *urlString = [NSString stringWithFormat:@"%@%@%@/", UAServer, @"/api/device_tokens/", deviceTokenString];
	NSURL *url = [NSURL URLWithString:urlString];

	NSMutableURLRequest *request = [[NSMutableURLRequest alloc] initWithURL:url];
	[request setHTTPMethod:@"PUT"];

	// handle the alias if we are sending one
	if( alias )
	{
		[request setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
		NSDictionary *dict = [NSDictionary dictionaryWithObject:alias forKey:@"alias"];
		NSData *data = [[EtceteraManager jsonFromObject:dict] dataUsingEncoding:NSUTF8StringEncoding];
		[request setHTTPBody:data];
	}

	// Authenticate to the server
	[request addValue:[NSString stringWithFormat:@"Basic %@",
					   [self base64forData:[[NSString stringWithFormat:@"%@:%@",
											 appKey,
											 appSecret] dataUsingEncoding: NSUTF8StringEncoding]]] forHTTPHeaderField:@"Authorization"];

	[[NSURLConnection connectionWithRequest:request delegate:self] start];
}


- (void)didFailToRegisterForRemoteNotificationsWithError:(NSNotification*)notification
{
	NSLog( @"didFailToRegisterForRemoteNotificationsWithError: %@", notification.userInfo );
	NSError *error = (NSError*)notification.userInfo;
	UnitySendMessage( "EtceteraManager", "remoteRegistrationDidFail", error.localizedDescription.UTF8String );
}


- (void)didReceiveRemoteNotification:(NSNotification*)notification
{
	NSLog( @"didReceiveRemoteNotification" );

	[self handleNotification:notification.userInfo isLaunchNotification:[UIApplication sharedApplication].applicationState == UIApplicationStateInactive];

	Class klass = NSClassFromString( @"GPlayRTRoomDelegate" );
	if( [klass respondsToSelector:@selector(handleRemoteNotification:)] )
		[klass performSelector:@selector(handleRemoteNotification:) withObject:notification.userInfo];
}


- (void)didReceiveLocalNotification:(NSNotification*)notification
{
	[self handleLocalNotification:(UILocalNotification*)notification.userInfo isLaunchNotification:[UIApplication sharedApplication].applicationState == UIApplicationStateInactive];
}



///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Public API

+ (void)registerForRemoteNotificationTypes:(NSNumber*)types
{
#if __IPHONE_OS_VERSION_MAX_ALLOWED >= 80000
	
	if( [[UIApplication sharedApplication] respondsToSelector:@selector(registerUserNotificationSettings:)] )
	{
		UIUserNotificationSettings *pushSettings = [UIUserNotificationSettings settingsForTypes:(UIUserNotificationTypeSound | UIUserNotificationTypeAlert | UIUserNotificationTypeBadge) categories:nil];
		[[UIApplication sharedApplication] registerUserNotificationSettings:pushSettings];
	}
	else
	{
		[[UIApplication sharedApplication] registerForRemoteNotificationTypes:[types intValue]];
	}

#else

	[[UIApplication sharedApplication] registerForRemoteNotificationTypes:[types intValue]];

#endif
}


+ (NSNumber*)enabledRemoteNotificationTypes
{
#if __IPHONE_OS_VERSION_MAX_ALLOWED >= 80000
	
	if( [[UIApplication sharedApplication] respondsToSelector:@selector(registerUserNotificationSettings:)] )
	{
		UIUserNotificationSettings *pushSettings = [[UIApplication sharedApplication] currentUserNotificationSettings];
		return @( pushSettings.types );
	}

#endif

	int val = [[UIApplication sharedApplication] enabledRemoteNotificationTypes];
	return [NSNumber numberWithInt:val];
}



///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Private

// From: http://www.cocoadev.com/index.pl?BaseSixtyFour
- (NSString*)base64forData:(NSData*)theData
{
    const uint8_t *input = (const uint8_t*)[theData bytes];
    NSInteger length = [theData length];

    static char table[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    NSMutableData *data = [NSMutableData dataWithLength:((length + 2) / 3) * 4];
    uint8_t *output = (uint8_t*)data.mutableBytes;

    NSInteger i;
    for( i = 0; i < length; i += 3 )
	{
        NSInteger value = 0;
        NSInteger j;
        for( j = i; j < (i + 3); j++ )
		{
            value <<= 8;

            if( j < length )
                value |= (0xFF & input[j]);
        }

        NSInteger theIndex = (i / 3) * 4;
        output[theIndex + 0] =                    table[(value >> 18) & 0x3F];
        output[theIndex + 1] =                    table[(value >> 12) & 0x3F];
        output[theIndex + 2] = (i + 1) < length ? table[(value >> 6)  & 0x3F] : '=';
        output[theIndex + 3] = (i + 2) < length ? table[(value >> 0)  & 0x3F] : '=';
    }

    return [[NSString alloc] initWithData:data encoding:NSASCIIStringEncoding];
}


- (void)handleLocalNotification:(UILocalNotification*)notification isLaunchNotification:(BOOL)isLaunchNotification
{
	NSMutableDictionary *dict = [NSMutableDictionary dictionaryWithObjectsAndKeys:notification.alertBody, @"alertBody",
								 notification.alertAction, @"alertAction",
								 [NSNumber numberWithInt:notification.applicationIconBadgeNumber], @"applicationIconBadgeNumber", nil];

	if( notification.alertLaunchImage )
		[dict setObject:notification.alertLaunchImage forKey:@"alertLaunchImage"];

	if( notification.userInfo )
		[dict setObject:notification.userInfo forKey:@"userInfo"];

	const char * managerMethod = isLaunchNotification ? "localNotificationWasReceivedAtLaunch" : "localNotificationWasReceived";

	NSString *json = [EtceteraManager jsonFromObject:dict];
	UnitySendMessage( "EtceteraManager", managerMethod, json.UTF8String );
}


- (void)handleNotification:(NSDictionary*)dict isLaunchNotification:(BOOL)isLaunchNotification
{
	NSDictionary *aps = [dict objectForKey:@"aps"];
	if( !aps )
		return;

	NSString *json = [EtceteraManager jsonFromObject:dict];

	const char * managerMethod = isLaunchNotification ? "remoteNotificationWasReceivedAtLaunch" : "remoteNotificationWasReceived";

	if( json )
		UnitySendMessage( "EtceteraManager", managerMethod, json.UTF8String );
	else
		UnitySendMessage( "EtceteraManager", managerMethod, "" );
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSURLConnection

- (void)connection:(NSURLConnection*)theConnection didReceiveResponse:(NSURLResponse*)response
{
	UnitySendMessage( "EtceteraManager", "urbanAirshipRegistrationDidSucceed", "" );

    NSLog( @"registered with UA: %@, %d",
		  [(NSHTTPURLResponse*)response allHeaderFields],
          [(NSHTTPURLResponse*)response statusCode] );
}


- (void)connection:(NSURLConnection*)theConnection didFailWithError:(NSError*)error
{
	UnitySendMessage( "EtceteraManager", "urbanAirshipRegistrationDidFail", [[error localizedDescription] UTF8String] );
	NSLog( @"Failed to register with UA: %@", error );
}


@end






#import "UnityAppController.h"


// Overrides iOS 8+ notification setup to properly work on iOS < 8 and 8+
@interface UnityAppController(PushFixes)
@end


@implementation UnityAppController(PushFixes)

#if __IPHONE_OS_VERSION_MAX_ALLOWED >= 80000

- (void)application:(UIApplication*)application didRegisterUserNotificationSettings:(UIUserNotificationSettings*)notificationSettings
{
	[application registerForRemoteNotifications];
}


#if !UNITY_USES_REMOTE_NOTIFICATIONS
- (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary*)userInfo
{
	AppController_SendNotificationWithArg(kUnityDidReceiveRemoteNotification, userInfo);
	UnitySendRemoteNotification(userInfo);
}


- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken
{
	AppController_SendNotificationWithArg(kUnityDidRegisterForRemoteNotificationsWithDeviceToken, deviceToken);
	UnitySendDeviceToken(deviceToken);
}


- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult result))handler
{
	AppController_SendNotificationWithArg(kUnityDidReceiveRemoteNotification, userInfo);
	UnitySendRemoteNotification(userInfo);
	if (handler)
	{
		handler(UIBackgroundFetchResultNoData);
	}
}


- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error
{
	AppController_SendNotificationWithArg(kUnityDidFailToRegisterForRemoteNotificationsWithError, error);
	UnitySendRemoteNotificationError(error);
}
#endif

#endif

@end





