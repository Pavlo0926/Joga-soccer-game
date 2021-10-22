//
//  UIImagePickerController+UIImagePickerControllerAdditions.m
//  Unity-iPhone


#import "UIImagePickerControllerAdditions.h"

#if __IPHONE_OS_VERSION_MAX_ALLOWED >= 80000
#import "Photos/Photos.h"
#endif


@implementation UIImagePickerController (Additions)

#if __IPHONE_OS_VERSION_MAX_ALLOWED >= 80000

+ (void)obtainPermissionForMediaSourceType:(UIImagePickerControllerSourceType)sourceType withSuccessHandler:(void (^) ())successHandler andFailure:(void (^) ())failureHandler
{
	if( sourceType == UIImagePickerControllerSourceTypePhotoLibrary || sourceType == UIImagePickerControllerSourceTypeSavedPhotosAlbum )
	{
		[PHPhotoLibrary requestAuthorization:^( PHAuthorizationStatus status )
		{
			switch( status )
			{
				case PHAuthorizationStatusAuthorized:
				{
					dispatch_async( dispatch_get_main_queue (), ^{ successHandler (); } );
					break;
				}

				case PHAuthorizationStatusRestricted:
				case PHAuthorizationStatusDenied:
				{
					if( failureHandler )
						dispatch_async( dispatch_get_main_queue (), ^{ failureHandler (); } );
					break;
				}

				default:
					break;
			}
		}];
	}
	else if( sourceType == UIImagePickerControllerSourceTypeCamera )
	{
		AVAuthorizationStatus status = [AVCaptureDevice authorizationStatusForMediaType:AVMediaTypeVideo];
		switch( status )
		{
			case AVAuthorizationStatusAuthorized:
			{
				dispatch_async( dispatch_get_main_queue (), ^{ successHandler (); });
				break;
			}

			case AVAuthorizationStatusNotDetermined:
			{
				[AVCaptureDevice requestAccessForMediaType:AVMediaTypeVideo completionHandler:^( BOOL granted )
				{
					if( granted )
					{
						dispatch_async( dispatch_get_main_queue (), ^{ successHandler (); } );
					}
					else
					{
						if( failureHandler )
							dispatch_async( dispatch_get_main_queue (), ^{ failureHandler (); } );
					}
				}];
				break;
			}

			case AVAuthorizationStatusDenied:
			case AVAuthorizationStatusRestricted:
			default:
			{
				if( failureHandler )
					dispatch_async( dispatch_get_main_queue (), ^{ failureHandler (); } );
				break;
			}
		}
	}
	else
	{
		NSAssert( NO, @"Permission type not found" );
	}
}

#else

// pre iOS 8 always return success
+ (void)obtainPermissionForMediaSourceType:(UIImagePickerControllerSourceType)sourceType withSuccessHandler:(void (^) ())successHandler andFailure:(void (^) ())failureHandler
{
	successHandler();
}

#endif

@end
