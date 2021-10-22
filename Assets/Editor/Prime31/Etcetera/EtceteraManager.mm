//
//  EtceteraManager.m
//  EtceteraTest
//
//  Created by Mike on 10/2/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "EtceteraManager.h"
#import "P31WebController.h"
#import "CNNAssetsPickerController.h"
#include <sys/socket.h>
#include <sys/sysctl.h>
#include <net/if.h>
#include <net/if_dl.h>
#import <CommonCrypto/CommonDigest.h>
#include <AVFoundation/AVFoundation.h>
#import <MobileCoreServices/MobileCoreServices.h>
#import "UIImagePickerControllerAdditions.h"

#if __IPHONE_OS_VERSION_MAX_ALLOWED >= 90000
#include <SafariServices/SafariServices.h>
#endif


#if UNITY_VERSION < 500
void UnityPause( bool pause );
#else
void UnityPause( int pause );
#endif

void UnitySendMessage( const char * className, const char * methodName, const char * param );

UIViewController *UnityGetGLViewController();


UIColor * ColorFromHex( int hexcolor )
{
	int r = ( hexcolor >> 24 ) & 0xFF;
	int g = ( hexcolor >> 16 ) & 0xFF;
	int b = ( hexcolor >> 8 ) & 0xFF;
	int a = hexcolor & 0xFF;

	return [UIColor colorWithRed:(r/255.0) green:(g/255.0) blue:(b/255.0) alpha:(a/255.0)];
}


// UIAlertView tags
#define kStandardAlertTag		1111
#define kSingleFieldAlertTag	2222
#define kTwoFieldAlertTag		3333
#define kRTAAlertTag			4444
#define kRTAAlertTagNoOptions	7777

// RTA defaults keys
#define kRTADontAskAgain			@"RTADontAskAgain"
#define kRTALastReviewedVersion		@"RTALastReviewedVersion"
#define kRTANextTimeToAsk			@"RTANextTimeToAsk"
#define kRTAFirstLaunchDate			@"RTAFirstLaunchDate"
#define kRTALastPromptDate			@"RTALastPromptDate"
#define kRTATimesLaunchedSinceAsked	@"RTATimesLaunchedSinceAsked"


@interface EtceteraManager()
@property (nonatomic, strong) UIAlertView* activeAlertView;
@property (nonatomic, strong) AVCaptureMovieFileOutput* movieFileOutput;
@property (nonatomic, strong) AVCaptureSession* captureSession;
@property (nonatomic, strong) AVCaptureVideoPreviewLayer* previewLayer;
@end



@implementation EtceteraManager

@synthesize urbanAirshipAppKey = _urbanAirshipAppKey, urbanAirshipAppSecret, iTunesUrl, scaledImageSize,
			borderColor, gradientStopOne, gradientStopTwo,
			popoverRect, pickerAllowsEditing, popoverViewController, urbanAirshipAlias,
			inlineWebView, maxPhotoPickerImageWidthOrHeight, handledActionSheetCallback;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Class Methods

+ (NSString*)stringWithNewUUID
{
    // Create a new UUID
    CFUUIDRef uuidObj = CFUUIDCreate( nil );

    // Get the string representation of the UUID
    CFStringRef newUUID = CFUUIDCreateString( nil, uuidObj );
    CFRelease( uuidObj );

	return (__bridge_transfer NSString *)newUUID;
}


+ (UIViewController*)unityViewController
{
	return UnityGetGLViewController();
}


+ (NSString*)jsonFromObject:(id)object
{
	NSError *error = nil;
	NSData *jsonData = [NSJSONSerialization dataWithJSONObject:object options:0 error:&error];
	
	if( jsonData && !error )
	{
		return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
	}
	else
	{
		NSLog( @"json serialization error: %@", [error localizedDescription] );
	}
	
	return @"{}";
}


+ (id)objectFromJson:(NSString*)json
{
	NSData *jsonData = [json dataUsingEncoding:NSUTF8StringEncoding];
	if( jsonData )
	{
		return [NSJSONSerialization JSONObjectWithData:jsonData options:0 error:nil];
	}
	else
	{
		NSLog( @"jsonData was null when converted from the passed in string" );
	}
	
    return [NSDictionary dictionary];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

+ (EtceteraManager*)sharedManager
{
	static EtceteraManager *sharedManager = nil;
	
	if( !sharedManager )
		sharedManager = [[EtceteraManager alloc] init];
	
	return sharedManager;
}


+ (void)sendMessage:(NSString*)method param:(NSString*)param
{
	UnitySendMessage( "EtceteraManager", method.UTF8String, param.UTF8String );
}


- (id)init
{
	if( ( self = [super init] ) )
	{
		_JPEGCompression = 0.8;
		self.pickerAllowsEditing = NO;
		self.scaledImageSize = 1.0f;
		self.popoverRect = CGRectMake( 20, 15, 10, 0 );
		self.maxPhotoPickerImageWidthOrHeight = 4096;
	}
	return self;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Private

- (UIImage*)constrainImageToMaxSize:(UIImage*)image
{
	// reset the scaledImageSize since we will be messing with the image size anyway
	self.scaledImageSize = 1.0;
	float scale = 1.0;
	
	// figure out our scale. it could be based on width or height so check both
	if( image.size.width > self.maxPhotoPickerImageWidthOrHeight )
		scale = self.maxPhotoPickerImageWidthOrHeight / image.size.width;
	else
		scale = self.maxPhotoPickerImageWidthOrHeight / image.size.height;
	
	return [self scaleImage:image toSize:CGSizeMake( image.size.width * scale, image.size.height * scale )];
}


- (UIImage*)scaleImage:(UIImage*)image toSize:(CGSize)size
{
	UIGraphicsBeginImageContext( size );
	[image drawInRect:CGRectMake( 0, 0, size.width, size.height )];
	UIImage *targetImage = UIGraphicsGetImageFromCurrentImageContext();
	UIGraphicsEndImageContext();
	
	image = targetImage;
	
	return image;
}


- (void)showViewControllerModallyInWrapper:(UIViewController*)viewController
{
	// pause the game
	UnityPause( true );
	
	// cancel the previous delayed call to dismiss the view controller if it exists
	[NSObject cancelPreviousPerformRequestsWithTarget:self];

	UIViewController *vc = UnityGetGLViewController();
	
	// show the mail composer on iPad in a form sheet
	if( UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad && [viewController isKindOfClass:[MFMailComposeViewController class]] )
		viewController.modalPresentationStyle = UIModalPresentationFormSheet;
	
	// show the view controller
	[vc presentViewController:viewController animated:YES completion:nil];
}


- (void)dismissWrappedController
{
	UnityPause( false );

	UIViewController *vc = UnityGetGLViewController();
	
	// No view controller? Get out of here.
	if( !vc )
		return;
	
	// dismiss the view controller
	[vc dismissViewControllerAnimated:YES completion:nil];

	// remove the wrapper view controller
	[self performSelector:@selector(removeAndReleaseViewControllerWrapper) withObject:nil afterDelay:1.0];
	
	UnitySendMessage( "EtceteraManager", "dismissingViewController", "" );
}


- (void)removeAndReleaseViewControllerWrapper
{
	// iPad might have a popover
	if( UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad && self.popoverViewController )
	{
		[self.popoverViewController dismissPopoverAnimated:YES];
		self.popoverViewController = nil;
	}
}


- (NSString*)iTunesUrlForAppId:(NSString*)appId
{
	if( [[[UIDevice currentDevice] systemVersion] floatValue] >= 7.0 )
	{
		return [NSString stringWithFormat:@"itms-apps://itunes.apple.com/app/id%@", appId];
	}
	else
	{
		return [NSString stringWithFormat:@"itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=%@", appId];
	}
}


- (void)image:(UIImage*)image didFinishSavingWithError:(NSError*)error contextInfo:(void*)contextInfo
{
	NSLog( @"image:didFinishSavingWithError:contextInfo: completed" );
	
	if( error )
	{
		NSLog( @"image:didFinishSavingWithError:contextInfo: %@", error );
		UnitySendMessage( "EtceteraManager", "saveImageToPhotoAlbumFailed", error.localizedDescription.UTF8String );
	}
	else
	{
		UnitySendMessage( "EtceteraManager", "saveImageToPhotoAlbumSucceeded", "" );
	}
}


- (void)video:(UIImage*)image didFinishSavingWithError:(NSError*)error contextInfo:(void*)contextInfo
{
	NSLog( @"video:didFinishSavingWithError:contextInfo: completed" );
	
	if( error )
	{
		NSLog( @"video:didFinishSavingWithError:contextInfo: %@", error );
		UnitySendMessage( "EtceteraManager", "saveVideoToPhotoAlbum", error.localizedDescription.UTF8String );
	}
	else
	{
		UnitySendMessage( "EtceteraManager", "saveVideoToPhotoAlbum", "" );
	}
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

// UIAlertView
- (void)showAlertWithTitle:(NSString*)title message:(NSString*)message buttons:(NSArray*)buttons
{
	UnityPause( true );
	self.activeAlertView = [[UIAlertView alloc] init];
	self.activeAlertView.delegate = self;
	self.activeAlertView.title = title;
	self.activeAlertView.message = message;
	
	for( NSString *b in buttons )
		[self.activeAlertView addButtonWithTitle:b];
	
	self.activeAlertView.tag = kStandardAlertTag;
	[self.activeAlertView show];
}


- (void)showPromptWithTitle:(NSString*)title message:(NSString*)message placeHolder:(NSString*)placeHolder autocorrect:(BOOL)autocorrect
{
	UnityPause( true );
	
	// we will use the fancy new style Alertview
	self.activeAlertView = [[UIAlertView alloc] initWithTitle:title
													  message:message
													 delegate:self
											cancelButtonTitle:NSLocalizedString( @"Cancel", nil )
											otherButtonTitles:NSLocalizedString( @"OK", nil ), nil];
	self.activeAlertView.alertViewStyle = UIAlertViewStylePlainTextInput;
	self.activeAlertView.tag = kSingleFieldAlertTag;
	
	// configure the text field
	UITextField *tf = [self.activeAlertView textFieldAtIndex:0];
	tf.placeholder = placeHolder;
	if( !autocorrect )
		tf.autocorrectionType = UITextAutocorrectionTypeNo;
	else
		tf.autocorrectionType = UITextAutocorrectionTypeDefault;
	
	[self.activeAlertView show];
}


- (void)showPromptWithTitle:(NSString*)title message:(NSString*)message placeHolder1:(NSString*)placeHolder1 placeHolder2:(NSString*)placeHolder2 autocorrect:(BOOL)autocorrect
{
	UnityPause( true );
	
	// we can use the fancy new Alertview if we are on iOS 5+
	self.activeAlertView = [[UIAlertView alloc] initWithTitle:title
														 message:message
														delegate:self
											   cancelButtonTitle:NSLocalizedString( @"Cancel", nil )
											   otherButtonTitles:NSLocalizedString( @"OK", nil ), nil];
	self.activeAlertView.alertViewStyle = UIAlertViewStyleLoginAndPasswordInput;
	self.activeAlertView.tag = kTwoFieldAlertTag;
	
	// configure the text field
	[self.activeAlertView textFieldAtIndex:0].placeholder = placeHolder1;
	[self.activeAlertView textFieldAtIndex:1].placeholder = placeHolder2;
	
	if( !autocorrect )
	{
		[self.activeAlertView textFieldAtIndex:0].autocorrectionType = UITextAutocorrectionTypeNo;
		[self.activeAlertView textFieldAtIndex:1].autocorrectionType = UITextAutocorrectionTypeNo;
	}
	else
	{
		[self.activeAlertView textFieldAtIndex:0].autocorrectionType = UITextAutocorrectionTypeDefault;
		[self.activeAlertView textFieldAtIndex:1].autocorrectionType = UITextAutocorrectionTypeDefault;
	}
	
	// If the second placeHolder has 'password' in it, make it a password field
	[self.activeAlertView textFieldAtIndex:1].secureTextEntry = [placeHolder2 hasPrefix:@"password"];
	
	[self.activeAlertView show];
}


- (void)dismissAlertView
{
	if( self.activeAlertView )
	{
		[self.activeAlertView dismissWithClickedButtonIndex:0 animated:NO];
		self.activeAlertView = nil;
		UnityPause( 0 );
	}
}


// P31WebController
- (void)showWebControllerWithUrl:(NSString*)url showingControls:(BOOL)showControls
{
	UnityPause( true );
	NSLog( @"ShowWebPage" );
	NSLog(@"ShowWebPage: %@",url);

	NSURL *testURL = [NSURL URLWithString:url];
	if (testURL && [testURL scheme] && [testURL host])
	{
		NSLog(@"ShowWebPage: url is valid");
	}
	{
		NSLog(@"ShowWebPage: url is not valid");
	}

	P31WebController *webCon = [[P31WebController alloc] initWithUrl:url showControls:showControls];
	UINavigationController *navCon = [[UINavigationController alloc] initWithRootViewController:webCon];
	[self showViewControllerModallyInWrapper:navCon];
}


- (void)showSafariViewControllerWithUrl:(NSString*)url
{
#if __IPHONE_OS_VERSION_MAX_ALLOWED >= 90000
	UnityPause( true );

	SFSafariViewController* vc = [[SFSafariViewController alloc] initWithURL:[NSURL URLWithString:url]];
	vc.delegate = (id<SFSafariViewControllerDelegate>)self;
	[UnityGetGLViewController() presentViewController:vc animated:YES completion:nil];
#endif
}


// Mail and SMS
- (BOOL)isEmailAvailable
{
	return [MFMailComposeViewController canSendMail];
}


- (BOOL)isSMSAvailable
{
	Class composerClass = NSClassFromString( @"MFMessageComposeViewController" );
	
	if( !composerClass )
		return NO;
	
	return [composerClass canSendText];
}


- (void)showMailComposerWithTo:(NSString*)toAddress subject:(NSString*)subject body:(NSString*)body isHTML:(BOOL)isHTML
{
	[self showMailComposerWithTo:toAddress
						 subject:subject
							body:body
						  isHTML:isHTML
					  attachment:nil
						mimeType:nil
						filename:nil];
}


- (void)showMailComposerWithTo:(NSString*)toAddress subject:(NSString*)subject body:(NSString*)body isHTML:(BOOL)isHTML attachment:(NSData*)data mimeType:(NSString*)mimeType filename:(NSString*)filename
{
	// early out if email isnt setup
	if( ![self isEmailAvailable] )
		return;
	
	MFMailComposeViewController *mailer = [[MFMailComposeViewController alloc] init];
	mailer.mailComposeDelegate = self;
	
	[mailer setSubject:subject];
	[mailer setMessageBody:body isHTML:isHTML];
	
	// Add the to address if we have one and it has an '@'
	if( toAddress && toAddress.length && [toAddress rangeOfString:@"@"].location != NSNotFound )
		[mailer setToRecipients:[NSArray arrayWithObject:toAddress]];
	
	// Add the attachment if we have one
	if( data && filename && mimeType )
		[mailer addAttachmentData:data mimeType:mimeType fileName:filename];
	
	[self showViewControllerModallyInWrapper:mailer];
}


- (void)showMailComposerWithTo:(NSString*)toAddress subject:(NSString*)subject body:(NSString*)body isHTML:(BOOL)isHTML imageAttachment:(NSData*)imageData
{
	// early out if email isnt setup
	if( ![self isEmailAvailable] )
		return;
	
	MFMailComposeViewController *mailer = [[MFMailComposeViewController alloc] init];
	mailer.mailComposeDelegate = self;
	
	[mailer setSubject:subject];
	[mailer setMessageBody:body isHTML:isHTML];
	
	// Add the to address if we have one and it has an '@'
	if( toAddress && toAddress.length && [toAddress rangeOfString:@"@"].location != NSNotFound )
		[mailer setToRecipients:[NSArray arrayWithObject:toAddress]];
	
	// Add the attachment if we have one
	if( imageData )
		[mailer addAttachmentData:imageData mimeType:@"image/png" fileName:@"image.png"];
	
	[self showViewControllerModallyInWrapper:mailer];
}


- (void)showSMSComposerWithBody:(NSString*)body
{
	[self showSMSComposerWithRecipients:nil body:body];
}


- (void)showSMSComposerWithRecipients:(NSArray*)recipients body:(NSString*)body
{
	if( ![self isSMSAvailable] )
		return;
	
	[UIApplication sharedApplication].statusBarHidden = NO;

	MFMessageComposeViewController *controller = [[MFMessageComposeViewController alloc] init];
	controller.body = body;
	controller.recipients = recipients;
	controller.messageComposeDelegate = self;
	
	[self showViewControllerModallyInWrapper:controller];
}


// Rate This App

- (BOOL)isAppEligibleForReviewWithLaunchCount:(int)launchesUntilPrompt hoursUntilFirstPrompt:(int)hoursUntilFirstPrompt
{
	NSUserDefaults* defaults = [NSUserDefaults standardUserDefaults];
	
	// If the user doesnt want us to ever ask this question than dont ask
	if( [defaults boolForKey:kRTADontAskAgain] )
		return NO;
	
	// Grab the current version from the bundle and the last reviewed version
	NSString *currentVersion = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleVersion"];
	NSString *lastReviewedVersion = [defaults stringForKey:kRTALastReviewedVersion];
	
	// If this version has been reviewed, than get out of here
	if( [lastReviewedVersion isEqualToString:currentVersion] )
		return NO;
	
	// Take care of setting the launch count and keeping it up to date
	int launchCount = [defaults integerForKey:kRTATimesLaunchedSinceAsked];
	[defaults setInteger:++launchCount forKey:kRTATimesLaunchedSinceAsked];

	// get date of first launch
	double firstLaunchDate = [defaults doubleForKey:kRTAFirstLaunchDate];
	if( firstLaunchDate == 0 )
	{
		NSLog( @"first launch date not set in prefs. setting it now" );
		firstLaunchDate = CFAbsoluteTimeGetCurrent();
		[defaults setDouble:firstLaunchDate forKey:kRTAFirstLaunchDate];
	}


	if( launchCount >= launchesUntilPrompt )
	{
		NSLog( @"launch count > launchesUntilPrompt. checking to see if first launch was greater than hoursUntilPrompt" );

		// wait at least n hours before opening
		if( CFAbsoluteTimeGetCurrent() >= firstLaunchDate + ( hoursUntilFirstPrompt * 60 * 60 ) )
		{
			NSLog( @"hoursUntilFirstPrompt have expired so we are clear to proceed" );

			// if we have prompted before, make sure enought time has elapssed before propmting again
			double lastPromptDate = [defaults doubleForKey:kRTALastPromptDate];

			if( lastPromptDate == 0 || CFAbsoluteTimeGetCurrent() >= lastPromptDate + ( _hoursBetweenPrompts * 60 * 60 ) )
			{
				NSLog( @"lastPromptDate is either 0 or enough time has elapsed to prompt again" );
				[defaults setDouble:CFAbsoluteTimeGetCurrent() forKey:kRTALastPromptDate];
				return YES;
			}
		}
	}

	return NO;
}


- (BOOL)askForReviewWithLaunchCount:(int)launchesUntilPrompt
			  hoursUntilFirstPrompt:(int)hoursUntilFirstPrompt
				hoursBetweenPrompts:(float)hoursBetweenPrompts
							  title:(NSString*)title message:(NSString*)message
						iTunesAppId:(NSString*)iTunesAppId
{
	// store this globally for easy access
	_hoursBetweenPrompts = hoursBetweenPrompts;
	
	// early out if we don't pass the isEligible test
	if( ![self isAppEligibleForReviewWithLaunchCount:launchesUntilPrompt hoursUntilFirstPrompt:hoursUntilFirstPrompt] )
		return NO;
	
	UIAlertView *alert = [[UIAlertView alloc] initWithTitle:title
													message:message
												   delegate:self
										  cancelButtonTitle:NSLocalizedString( @"Remind me later", nil )
										  otherButtonTitles:NSLocalizedString( @"Yes, rate it!", nil ), NSLocalizedString( @"Don't ask again", nil ), nil];
	alert.tag = kRTAAlertTag;
	[alert show];
	
	// Save the iTunesUrl for now
	self.iTunesUrl = [self iTunesUrlForAppId:iTunesAppId];

	return YES;
}


- (void)askForReviewWithTitle:(NSString*)title message:(NSString*)message iTunesAppId:(NSString*)iTunesAppId
{
	UIAlertView *alert = [[UIAlertView alloc] initWithTitle:title
													message:message
												   delegate:self
										  cancelButtonTitle:NSLocalizedString( @"Cancel", nil )
										  otherButtonTitles:NSLocalizedString( @"OK!", nil ), nil];
	alert.tag = kRTAAlertTagNoOptions;
	[alert show];
	
	// Save the iTunesUrl for now
	self.iTunesUrl = [self iTunesUrlForAppId:iTunesAppId];
}


- (void)openAppStoreReviewPageWithiTunesAppId:(NSString*)iTunesAppId
{
	[[UIApplication sharedApplication] openURL:[NSURL URLWithString:[self iTunesUrlForAppId:iTunesAppId]]];
}


// Photo Library and Camera
- (void)showPicker:(UIImagePickerControllerSourceType)type
{
	UIImagePickerController *picker = [[UIImagePickerController alloc] init];
	picker.delegate = self;
	picker.sourceType = type;
	picker.allowsEditing = self.pickerAllowsEditing;
	
	// for video we have to change some stuff
	if( self.pickerWantsVideo )
	{
		picker.allowsEditing = NO;
		picker.mediaTypes = @[(NSString*)kUTTypeMovie];
	}
	
	picker.mediaTypes = [UIImagePickerController availableMediaTypesForSourceType:
        UIImagePickerControllerSourceTypeCamera];
		
	NSLog(@"MediaTypes: %@", picker.mediaTypes);
	
	
	if( UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad )
	{
		self.popoverViewController = [[UIPopoverController alloc] initWithContentViewController:picker];
		self.popoverViewController.delegate = self;
		//picker.modalInPopover = YES;
		
		// Display the popover
		[self.popoverViewController presentPopoverFromRect:popoverRect
												inView:UnityGetGLViewController().view
							  permittedArrowDirections:UIPopoverArrowDirectionAny
											  animated:YES];
	}
	else
	{
		[self showViewControllerModallyInWrapper:picker];
	}
}


- (void)promptForMultiplePhotos:(int)maxNumberOfPhotos
{
	CNNAssetsPickerController *picker = [[CNNAssetsPickerController alloc] init];
	picker.delegate = self;
	picker.maximumNumberOfSelection = maxNumberOfPhotos;
	picker.assetsFilter = [ALAssetsFilter allAssets];
	
	UnityPause( true );
	[UnityGetGLViewController() presentViewController:picker animated:YES completion:nil];
}


- (void)popoverControllerDidDismissPopover:(UIPopoverController*)popoverController
{
	self.popoverViewController = nil;
	UnityPause( false );
	
	UnitySendMessage( "EtceteraManager", "imagePickerDidCancel", "" );
}


- (void)promptForPhotoWithType:(PhotoType)type
{
	UnityPause( true );

	// No need to give a choice for devices with no camera
	if( ![UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeCamera] )
	{
		[self showPicker:UIImagePickerControllerSourceTypePhotoLibrary];
		return;
	}
	
	if( type == PhotoTypeAlbum )
	{
		[self showPicker:UIImagePickerControllerSourceTypePhotoLibrary];
		return;
	}
	else if( type == PhotoTypeCamera )
	{
		[self showPicker:UIImagePickerControllerSourceTypeCamera];
		return;
	}
	
	NSString* buttonOne = NSLocalizedString( @"Take Photo", nil );
	NSString* buttonTwo = NSLocalizedString( @"Choose Existing Media", nil );
	
	//NSString* buttonOne = NSLocalizedString( @"Take Photo", nil );
	//NSString* buttonTwo = NSLocalizedString( @"Choose Existing Photo", nil );
	//if( self.pickerWantsVideo )
	//{
	//	buttonOne = NSLocalizedString( @"Take Video", nil );
	//	buttonTwo = NSLocalizedString( @"Choose Existing Video", nil );
	//}
	
	UIActionSheet *sheet = [[UIActionSheet alloc] initWithTitle:nil
													   delegate:self
											  cancelButtonTitle:NSLocalizedString( @"Cancel", nil )
										 destructiveButtonTitle:nil
											  otherButtonTitles:buttonOne, buttonTwo, nil];
	self.handledActionSheetCallback = NO;
	
	if( UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad )
		[sheet showFromRect:popoverRect inView:UnityGetGLViewController().view animated:YES];
	else
		[sheet showInView:UnityGetGLViewController().view];
}


// Inline web view
- (void)inlineWebViewShowWithFrame:(CGRect)frame
{
	if( inlineWebView )
		[self inlineWebViewClose];
	
	inlineWebView = [[UIWebView alloc] initWithFrame:frame];
	inlineWebView.scalesPageToFit = YES;
	inlineWebView.autoresizingMask = UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight;
	[UnityGetGLViewController().view addSubview:inlineWebView];
}


- (void)inlineWebViewClose
{
	[inlineWebView removeFromSuperview];
	self.inlineWebView = nil;
}


- (void)inlineWebViewSetUrl:(NSString*)urlString
{
	NSURLRequest *request = [NSURLRequest requestWithURL:[NSURL URLWithString:urlString]];
	[inlineWebView loadRequest:request];
}


- (void)inlineWebViewSetFrame:(CGRect)frame
{
	[UIView beginAnimations:nil context:NULL];
	inlineWebView.frame = frame;
	[UIView commitAnimations];
}



// Camera capture
- (AVCaptureDevice*)getCaptureDevice:(bool)useFrontCamera
{
	// try to find a front camrea if desired
	if( useFrontCamera )
	{
		for( AVCaptureDevice* device in [AVCaptureDevice devicesWithMediaType:AVMediaTypeVideo] )
		{
			if( device.position == AVCaptureDevicePositionFront )
				return device;
		}
	}

	return [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];
}


- (void)startCameraCaptureWithFrontFacingCamera:(bool)useFrontCamera previewFrame:(CGRect)frame outputFileURL:(NSURL*)outputFileURL
{
	if( self.captureSession )
	{
		NSLog( @"there is already a capture session running!" );
		return;
	}

	self.captureSession = [[AVCaptureSession alloc] init];
	[self.captureSession beginConfiguration];
	self.captureSession.sessionPreset = AVCaptureSessionPresetLow;

	
	// fetch the Unity GLView and stick our preview layer in it
	self.previewLayer = [[AVCaptureVideoPreviewLayer alloc] initWithSession:self.captureSession];
	self.previewLayer.videoGravity = AVLayerVideoGravityResizeAspectFill;
	self.previewLayer.frame = frame;
	
	dispatch_after( dispatch_time( DISPATCH_TIME_NOW, 1 * NSEC_PER_SEC ), dispatch_get_main_queue(),
	^{
		
	});

	[UnityGetGLView().layer addSublayer:self.previewLayer];


	// add our camera as the input device
	AVCaptureDevice *device = [self getCaptureDevice:useFrontCamera];

	NSError* error = nil;
	AVCaptureDeviceInput* input = [AVCaptureDeviceInput deviceInputWithDevice:device error:&error];
	if( !input )
	{
		NSLog( @"ERROR: trying to open camera: %@", error );
		self.captureSession = nil;
		return;
	}

	// add our microphone for audio
	AVCaptureDevice* audioDevice = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeAudio];
	AVCaptureDeviceInput* audioInput = [AVCaptureDeviceInput deviceInputWithDevice:audioDevice error:nil];


	// setup our file for recording
	self.movieFileOutput = [[AVCaptureMovieFileOutput alloc] init];


	// configure the capture session
	if( [self.captureSession canAddInput:input] )
		[self.captureSession addInput:input];

	if( [self.captureSession canAddInput:audioInput] )
		[self.captureSession addInput:audioInput];

	if( [self.captureSession canAddOutput:self.movieFileOutput] )
		[self.captureSession addOutput:self.movieFileOutput];


	// set orientation on our goods
	AVCaptureConnection* conn = [self.movieFileOutput connectionWithMediaType:AVMediaTypeVideo];

	UIInterfaceOrientation currentOrientation = UnityGetGLViewController().interfaceOrientation;
	if( currentOrientation == UIInterfaceOrientationPortrait )
	{
		// one day, when Apple fixes the bugs with AVCaptureController.videoOrientation switch this over to use it
		self.previewLayer.orientation = AVCaptureVideoOrientationPortrait;
		[conn setVideoOrientation:AVCaptureVideoOrientationPortrait];
	}
	else if( currentOrientation == UIInterfaceOrientationPortraitUpsideDown )
	{
		self.previewLayer.orientation = AVCaptureVideoOrientationPortraitUpsideDown;
		[conn setVideoOrientation:AVCaptureVideoOrientationPortraitUpsideDown];
	}
	else if( currentOrientation == UIInterfaceOrientationLandscapeLeft )
	{
		self.previewLayer.orientation = AVCaptureVideoOrientationLandscapeLeft;
		[conn setVideoOrientation:AVCaptureVideoOrientationLandscapeLeft];
	}
	else if( currentOrientation == UIInterfaceOrientationLandscapeRight )
	{
		self.previewLayer.orientation = AVCaptureVideoOrientationLandscapeRight;
		[conn setVideoOrientation:AVCaptureVideoOrientationLandscapeRight];
	}

	// commit and get this sucker started
	[self.captureSession commitConfiguration];
	[self.captureSession startRunning];
	[self.movieFileOutput startRecordingToOutputFileURL:outputFileURL recordingDelegate:self];
}


- (void)stopCameraCapture
{
	// guard against calling when we arent recording
	if( !self.captureSession )
		return;

	self.previewLayer.session = nil;
	[self.previewLayer removeFromSuperlayer];
	self.previewLayer = nil;
	
	// stop recording
	[self.movieFileOutput stopRecording];
	self.movieFileOutput = nil;

	// clean up the capture session
	for( AVCaptureDeviceInput *input in self.captureSession.inputs )
		[self.captureSession removeInput:input];
	
	for( AVCaptureOutput *output in self.captureSession.outputs )
		[self.captureSession removeOutput:output];
	[self.captureSession stopRunning];
	self.captureSession = nil;
}


- (void)setCameraCaptureFrame:(CGRect)frame
{
	// guard against calling when we arent recording
	if( !self.captureSession )
		return;

	[self.previewLayer setFrame:frame];
}



///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark AVCaptureFileOutputRecordingDelegate

- (void)captureOutput:(AVCaptureFileOutput *)captureOutput didStartRecordingToOutputFileAtURL:(NSURL *)fileURL fromConnections:(NSArray *)connections
{
	NSLog( @"recording started" );
}


- (void)captureOutput:(AVCaptureFileOutput *)captureOutput didPauseRecordingToOutputFileAtURL:(NSURL *)fileURL fromConnections:(NSArray *)connections
{
	NSLog( @"recording paused" );
}


- (void)captureOutput:(AVCaptureFileOutput *)captureOutput didResumeRecordingToOutputFileAtURL:(NSURL *)fileURL fromConnections:(NSArray *)connections
{
	NSLog( @"recording resumed" );
}


- (void)captureOutput:(AVCaptureFileOutput *)captureOutput didFinishRecordingToOutputFileAtURL:(NSURL *)outputFileURL fromConnections:(NSArray *)connections error:(NSError *)error
{
	BOOL didSucceed = YES;
	if( error && error.userInfo && [[error.userInfo allKeys] containsObject:AVErrorRecordingSuccessfullyFinishedKey] )
	{
		didSucceed = [error.userInfo[AVErrorRecordingSuccessfullyFinishedKey] boolValue];
		NSLog( @"Recording finished but we were given an error. The error has the AVErrorRecordingSuccessfullyFinishedKey set as: %@. The full error will be printed below.", didSucceed ? @"YES" : @"NO" );
	}
	NSLog( @"recording completed. An error being present does NOT indicate the recording failed. error: %@", error );

	if( didSucceed )
		UnitySendMessage( "EtceteraManager", "videoRecordingSucceeded", outputFileURL.absoluteString.UTF8String );
	else
		UnitySendMessage( "EtceteraManager", "videoRecordingFailed", error.localizedDescription.UTF8String );
}



///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - SFSafariViewControllerDelegate

- (void)safariViewControllerDidFinish:(UIViewController*)controller
{
	[UnityGetGLViewController() dismissViewControllerAnimated:YES completion:nil];
	UnityPause( NO );
}



///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark UIActionSheetDelegate

- (void)actionSheet:(UIActionSheet*)actionSheet didDismissWithButtonIndex:(NSInteger)buttonIndex
{
	// guard against iOS calling didDismiss two times
	if( self.handledActionSheetCallback )
		return;

	self.handledActionSheetCallback = YES;

	if( buttonIndex == 0 )
	{
		[UIImagePickerController obtainPermissionForMediaSourceType:UIImagePickerControllerSourceTypeCamera
			withSuccessHandler:^{
				[self showPicker:UIImagePickerControllerSourceTypeCamera];
			}
			andFailure:^{
				NSLog( @"failed camera permissions" );
				UIAlertView *alert = [[UIAlertView alloc] initWithTitle:NSLocalizedString( @"Camera Access Denied", nil )
																message:NSLocalizedString( @"Open the Settings app to allow us to access the camera", nil )
															   delegate:nil
													  cancelButtonTitle:NSLocalizedString( @"OK", nil )
													  otherButtonTitles:nil];
    			[alert show];
				UnityPause( false );
			}];
	}
	else if( buttonIndex == 1 )
	{
		[UIImagePickerController obtainPermissionForMediaSourceType:UIImagePickerControllerSourceTypePhotoLibrary
			 withSuccessHandler:^{
				 [self showPicker:UIImagePickerControllerSourceTypePhotoLibrary];
			 }
			 andFailure:^{
				 NSLog( @"failed photo library" );
				 UIAlertView *alert = [[UIAlertView alloc] initWithTitle:NSLocalizedString( @"Photo Library Access Denied", nil )
																 message:NSLocalizedString( @"Open the Settings app to allow us to access the photo library", nil )
																delegate:nil
													   cancelButtonTitle:NSLocalizedString( @"OK", nil )
													   otherButtonTitles:nil];
				 [alert show];
				 UnityPause( false );
			 }];
	}
	else // Cancelled
	{
		UnityPause( false );
		UnitySendMessage( "EtceteraManager", "imagePickerDidCancel", "" );
	}
}



///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark UIImagePickerControllerDelegate

- (void)imagePickerController:(UIImagePickerController*)picker didFinishPickingMediaWithInfo:(NSDictionary*)info
{
	// To print out all key-value pairs in the NSDictionary info
	for(id key in info)
		NSLog(@"key=%@ value=%@", key, [info objectForKey:key]);
	
	NSLog(@"%@", [info objectForKey:UIImagePickerControllerMediaType]);
	
    if( [[info objectForKey:UIImagePickerControllerMediaType]  isEqual: @"public.movie"] )
	{
		dispatch_async( dispatch_get_global_queue( DISPATCH_QUEUE_PRIORITY_LOW, 0 ),
		^{
			NSString* sourcePath = [info objectForKey:UIImagePickerControllerMediaURL];
			
			// Get a filepath pointing to the docs directory
			NSArray *dirs = NSSearchPathForDirectoriesInDomains( NSDocumentDirectory, NSUserDomainMask, YES );
			NSString *filename = [NSString stringWithFormat:@"%@.%@", [EtceteraManager stringWithNewUUID], sourcePath.lastPathComponent];
			NSString *filePath = [[dirs objectAtIndex:0] stringByAppendingPathComponent:filename];
			
			[[NSFileManager defaultManager] copyItemAtPath:sourcePath toPath:filePath error:nil];
			dispatch_async( dispatch_get_main_queue(),
		   ^{
			   UnitySendMessage( "EtceteraManager", "imageSavedToDocuments", filePath.UTF8String );
		   });
		});
	}
	else
	{
		// Grab the image and write it to disk
		UIImage *image;
		
		if( self.pickerAllowsEditing )
			image = [info objectForKey:UIImagePickerControllerEditedImage];
		else
			image = [info objectForKey:UIImagePickerControllerOriginalImage];
		
		NSLog( @"picker got image with orientation: %i", image.imageOrientation );

		// Do the save and resize on a background thread
		dispatch_async( dispatch_get_global_queue( DISPATCH_QUEUE_PRIORITY_LOW, 0 ),
		^{
			[self processImageFromImagePicker:image];
		});
	}

	// Dimiss the pickerController
	[self dismissWrappedController];
}


- (void)processImageFromImagePicker:(UIImage*)image
{
	@autoreleasepool
	{
		// here we constrain to the maxPhotoPickerImageWidthOrHeight
		if( image.size.width > self.maxPhotoPickerImageWidthOrHeight || image.size.height > self.maxPhotoPickerImageWidthOrHeight )
			image = [self constrainImageToMaxSize:image];
		

		// Get a filepath pointing to the docs directory
		NSArray *dirs = NSSearchPathForDirectoriesInDomains( NSDocumentDirectory, NSUserDomainMask, YES );
		NSString *filename = [NSString stringWithFormat:@"%@.jpg", [EtceteraManager stringWithNewUUID]];
		NSString *filePath = [[dirs objectAtIndex:0] stringByAppendingPathComponent:filename];
		

		// Shrink the monster image down
		if( self.scaledImageSize != 1.0f )
		{
			float width = image.size.width * self.scaledImageSize;
			float height = image.size.height * self.scaledImageSize;
			image = [self scaleImage:image toSize:CGSizeMake( width, height )];
		}

		[UIImageJPEGRepresentation( [image imageWithImageDataMatchingOrientation], _JPEGCompression ) writeToFile:filePath atomically:NO];
		
		dispatch_async( dispatch_get_main_queue(),
		^{
			UnitySendMessage( "EtceteraManager", "imageSavedToDocuments", filePath.UTF8String );
		});
	}
}


- (void)imagePickerControllerDidCancel:(UIImagePickerController*)picker
{
	// dismiss the wrapper, unpause and notifiy Unity what happened
	[self dismissWrappedController];
	UnityPause( false );
	UnitySendMessage( "EtceteraManager", "imagePickerDidCancel", "" );
}



///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark UIAlertViewDelegate

- (void)alertView:(UIAlertView*)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
	UnityPause( false );
	self.activeAlertView = nil;

	// always dump the button clicked
	NSString *title = [alertView buttonTitleAtIndex:buttonIndex];
	UnitySendMessage( "EtceteraManager", "alertViewClickedButton", [title UTF8String] );

	if( alertView.tag == kRTAAlertTag || alertView.tag == kRTAAlertTagNoOptions ) // Rate this app. We can share with the no options version
	{
		switch( buttonIndex )
		{
			case 0: // remind me later
			{
				const double nextTime = CFAbsoluteTimeGetCurrent() + _hoursBetweenPrompts * 60 * 60;
				[[NSUserDefaults standardUserDefaults] setDouble:nextTime forKey:kRTANextTimeToAsk];
				break;
			}
			case 1: // rate it now
			{
				// grab the current version and save it in the defaults
				NSString *version = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleVersion"];
				[[NSUserDefaults standardUserDefaults] setValue:version forKey:kRTALastReviewedVersion];
				
				[[UIApplication sharedApplication] openURL:[NSURL URLWithString:self.iTunesUrl]];
				break;
			}
			case 2: // don't ask again
			{
				[[NSUserDefaults standardUserDefaults] setBool:YES forKey:kRTADontAskAgain];
				break;
			}
		}
		
		// reset the launch count
		self.iTunesUrl = nil;
		[[NSUserDefaults standardUserDefaults] setInteger:0 forKey:kRTATimesLaunchedSinceAsked];
	}
	else if( alertView.tag == kSingleFieldAlertTag || alertView.tag == kTwoFieldAlertTag ) // single field prompt
	{
		if( buttonIndex == 0 )
		{
			UnitySendMessage( "EtceteraManager", "alertPromptCancelled", "" );
		}
		else
		{
			NSString *returnText;
			if( alertView.tag == kSingleFieldAlertTag )
				returnText = [alertView textFieldAtIndex:0].text;
			else
				returnText = [NSString stringWithFormat:@"%@|||%@", [alertView textFieldAtIndex:0].text, [alertView textFieldAtIndex:1].text];
			
			UnitySendMessage( "EtceteraManager", "alertPromptEnteredText", [returnText UTF8String] );
		}
	}
}
						


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark MFMailComposerDelegate

- (void)mailComposeController:(MFMailComposeViewController*)controller didFinishWithResult:(MFMailComposeResult)result error:(NSError*)error
{
	[self dismissWrappedController];
	
	NSString *resultString = nil;
	
	switch( result )
	{
		case MFMailComposeResultCancelled:
			resultString = @"Cancelled";
			break;
		case MFMailComposeResultSaved:
			resultString = @"Saved";
			break;
		case MFMailComposeResultSent:
			resultString = @"Sent";
			break;
		case MFMailComposeResultFailed:
			resultString = @"Failed";
			break;
		default:
			resultString = @"";
	}
	
	UnitySendMessage( "EtceteraManager", "mailComposerFinishedWithResult", [resultString UTF8String] );
}



///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark MFMessageComposeViewControllerDelegate

- (void)messageComposeViewController:(MFMessageComposeViewController*)controller didFinishWithResult:(MessageComposeResult)result
{
	[self dismissWrappedController];
	
	NSString *resultString = nil;
	
	switch( result )
	{
		case MessageComposeResultCancelled:
			resultString = @"Cancelled";
			break;
		case MessageComposeResultSent:
			resultString = @"Sent";
			break;
		case MessageComposeResultFailed:
			resultString = @"Failed";
			break;
		default:
			resultString = @"";
	}
	
	UnitySendMessage( "EtceteraManager", "smsComposerFinishedWithResult", [resultString UTF8String] );
	
	[UIApplication sharedApplication].statusBarHidden = YES;
}



///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - AssetsPickerDelegate

- (void)assetsPickerController:(CNNAssetsPickerController*)picker didFinishPickingAssets:(NSArray*)assets
{
	if( assets.count == 0 )
	{
		NSLog( @"no images chosen" );
		UnitySendMessage( "EtceteraManager", "imagePickerDidCancel", "" );
	}
	
	NSArray *dirs = NSSearchPathForDirectoriesInDomains( NSDocumentDirectory, NSUserDomainMask, YES );
	for( ALAsset *asset in assets )
	{
		// prep the image
		ALAssetRepresentation *representation = asset.defaultRepresentation;
		UIImage *image = [UIImage imageWithCGImage:representation.fullResolutionImage
															scale:self.scaledImageSize
														orientation:(UIImageOrientation)representation.orientation];
		
		// save to disk
		NSString *filename = [NSString stringWithFormat:@"%@.jpg", [EtceteraManager stringWithNewUUID]];
		NSString *filePath = [[dirs objectAtIndex:0] stringByAppendingPathComponent:filename];
		
		[UIImageJPEGRepresentation( [image imageWithImageDataMatchingOrientation], _JPEGCompression ) writeToFile:filePath atomically:NO];
		UnitySendMessage( "EtceteraManager", "imageSavedToDocuments", filePath.UTF8String );
	}
	
	UnityPause( false );
}


- (void)assetsPickerControllerDidCancel:(CNNAssetsPickerController*)picker
{
	// dismiss the wrapper, unpause and notifiy Unity what happened
	[self dismissWrappedController];
	UnityPause( false );
	UnitySendMessage( "EtceteraManager", "imagePickerDidCancel", "" );
}

@end




@implementation UIImage(OrientationAdditions)

- (UIImage*)imageWithImageDataMatchingOrientation
{
    // no-op if the orientation is already correct
    if( self.imageOrientation == UIImageOrientationUp )
		return self;
	
    // We need to calculate the proper transformation to make the image upright.
    // We do it in 2 steps: Rotate if Left/Right/Down, and then flip if Mirrored.
    CGAffineTransform transform = CGAffineTransformIdentity;
	
    switch( self.imageOrientation )
	{
        case UIImageOrientationDown:
        case UIImageOrientationDownMirrored:
            transform = CGAffineTransformTranslate( transform, self.size.width, self.size.height );
            transform = CGAffineTransformRotate( transform, M_PI );
            break;
			
        case UIImageOrientationLeft:
        case UIImageOrientationLeftMirrored:
            transform = CGAffineTransformTranslate( transform, self.size.width, 0 );
            transform = CGAffineTransformRotate( transform, M_PI_2 );
            break;
			
        case UIImageOrientationRight:
        case UIImageOrientationRightMirrored:
            transform = CGAffineTransformTranslate( transform, 0, self.size.height );
            transform = CGAffineTransformRotate( transform, -M_PI_2 );
            break;
		default:
			NSLog( @"unsupported image orientation" );
			break;
    }
	
    switch( self.imageOrientation )
	{
        case UIImageOrientationUpMirrored:
        case UIImageOrientationDownMirrored:
            transform = CGAffineTransformTranslate( transform, self.size.width, 0 );
            transform = CGAffineTransformScale( transform, -1, 1 );
            break;
			
        case UIImageOrientationLeftMirrored:
        case UIImageOrientationRightMirrored:
            transform = CGAffineTransformTranslate( transform, self.size.height, 0 );
            transform = CGAffineTransformScale( transform, -1, 1 );
            break;
		default:
			NSLog( @"unsupported image orientation" );
			break;
    }
	
    // Now we draw the underlying CGImage into a new context, applying the transform calculated above.
    CGContextRef ctx = CGBitmapContextCreate( NULL, self.size.width, self.size.height,
                                             CGImageGetBitsPerComponent( self.CGImage ), 0,
                                             CGImageGetColorSpace( self.CGImage ),
                                             CGImageGetBitmapInfo( self.CGImage ) );
    CGContextConcatCTM( ctx, transform );
    switch( self.imageOrientation )
	{
        case UIImageOrientationLeft:
        case UIImageOrientationLeftMirrored:
        case UIImageOrientationRight:
        case UIImageOrientationRightMirrored:
            CGContextDrawImage( ctx, CGRectMake( 0, 0, self.size.height, self.size.width ), self.CGImage );
            break;
			
        default:
            CGContextDrawImage( ctx, CGRectMake( 0, 0, self.size.width, self.size.height ), self.CGImage );
            break;
    }
	
    // And now we just create a new UIImage from the drawing context
    CGImageRef cgimg = CGBitmapContextCreateImage(ctx);
    UIImage *img = [UIImage imageWithCGImage:cgimg];
    CGContextRelease( ctx );
    CGImageRelease( cgimg );
	
    return img;
}

@end
