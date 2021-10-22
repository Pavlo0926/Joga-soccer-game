//
//  EtceteraManager.h
//  EtceteraTest
//
//  Created by Mike on 10/2/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <MessageUI/MessageUI.h>
#import <MessageUI/MFMailComposeViewController.h>
#import "CNNAssetsPickerController.h"
#import <AVFoundation/AVFoundation.h>


#define SYSTEM_VERSION_LESS_THAN( v )  ( [[[UIDevice currentDevice] systemVersion] compare:v options:NSNumericSearch] == NSOrderedAscending )

typedef enum
{
	PhotoTypeCamera,
	PhotoTypeAlbum,
	PhotoTypeBoth
} PhotoType;


@interface EtceteraManager : NSObject <UIAlertViewDelegate, MFMailComposeViewControllerDelegate, MFMessageComposeViewControllerDelegate, UIPopoverControllerDelegate,
										UINavigationControllerDelegate, UIImagePickerControllerDelegate, UIActionSheetDelegate, CNNAssetsPickerControllerDelegate, AVCaptureFileOutputRecordingDelegate>
{
@private
	float _hoursBetweenPrompts;
}
@property (nonatomic, strong) NSString *urbanAirshipAppKey;
@property (nonatomic, strong) NSString *urbanAirshipAppSecret;
@property (nonatomic, strong) NSString *urbanAirshipAlias;
@property (nonatomic, strong) NSString *iTunesUrl;
@property (nonatomic, assign) float scaledImageSize;
@property (nonatomic, assign) int maxPhotoPickerImageWidthOrHeight;
@property (nonatomic, assign) CGRect popoverRect;
@property (nonatomic, assign) BOOL pickerAllowsEditing;
@property (nonatomic, assign) BOOL pickerWantsVideo;
@property (nonatomic, assign) float JPEGCompression;
@property (nonatomic, strong) UIPopoverController *popoverViewController;

@property (nonatomic, strong) UIView *keyboardView;
@property (nonatomic, strong) UIColor *borderColor;
@property (nonatomic, strong) UIColor *gradientStopOne;
@property (nonatomic, strong) UIColor *gradientStopTwo;
@property (nonatomic, strong) UIWebView *inlineWebView;
@property (nonatomic, assign) BOOL handledActionSheetCallback;


+ (EtceteraManager*)sharedManager;

+ (NSString*)stringWithNewUUID;

+ (UIViewController*)unityViewController;

+ (void)sendMessage:(NSString*)method param:(NSString*)param;

+ (NSString*)jsonFromObject:(id)object;

+ (id)objectFromJson:(NSString*)json;

- (void)dismissWrappedController;


// UIAlertView
- (void)showAlertWithTitle:(NSString*)title message:(NSString*)message buttons:(NSArray*)buttons;

- (void)showPromptWithTitle:(NSString*)title message:(NSString*)message placeHolder:(NSString*)placeHolder autocorrect:(BOOL)autocorrect;

- (void)showPromptWithTitle:(NSString*)title message:(NSString*)message placeHolder1:(NSString*)placeHolder1 placeHolder2:(NSString*)placeHolder2 autocorrect:(BOOL)autocorrect;

- (void)dismissAlertView;


// P31WebController
- (void)showWebControllerWithUrl:(NSString*)url showingControls:(BOOL)showControls;

- (void)showSafariViewControllerWithUrl:(NSString*)url;


// Mail
- (BOOL)isEmailAvailable;

- (BOOL)isSMSAvailable;

- (void)showMailComposerWithTo:(NSString*)toAddress subject:(NSString*)subject body:(NSString*)body isHTML:(BOOL)isHTML;

- (void)showMailComposerWithTo:(NSString*)toAddress subject:(NSString*)subject body:(NSString*)body isHTML:(BOOL)isHTML attachment:(NSData*)data mimeType:(NSString*)mimeType filename:(NSString*)filename;

- (void)showSMSComposerWithBody:(NSString*)body;

- (void)showSMSComposerWithRecipients:(NSArray*)recipients body:(NSString*)body;

- (void)showSMSComposerWithBody:(NSString*)body;

- (void)showSMSComposerWithRecipients:(NSArray*)recipients body:(NSString*)body;


// Rate this app
- (BOOL)askForReviewWithLaunchCount:(int)launchesUntilPrompt
			  hoursUntilFirstPrompt:(int)hoursUntilFirstPrompt
				hoursBetweenPrompts:(float)hoursBetweenPrompts
							  title:(NSString*)title message:(NSString*)message
						iTunesAppId:(NSString*)iTunesAppId;

- (void)askForReviewWithTitle:(NSString*)title message:(NSString*)message iTunesAppId:(NSString*)iTunesAppId;

- (void)openAppStoreReviewPageWithiTunesAppId:(NSString*)iTunesAppId;


// Photo and Photo Library
- (void)promptForPhotoWithType:(PhotoType)type;

- (void)promptForMultiplePhotos:(int)maxNumberOfPhotos;


// Inline web view
- (void)inlineWebViewShowWithFrame:(CGRect)frame;

- (void)inlineWebViewClose;

- (void)inlineWebViewSetUrl:(NSString*)urlString;

- (void)inlineWebViewSetFrame:(CGRect)frame;


// Camera Capture
- (void)startCameraCaptureWithFrontFacingCamera:(bool)useFrontCamera previewFrame:(CGRect)frame outputFileURL:(NSURL*)outputFileURL;

- (void)stopCameraCapture;

- (void)setCameraCaptureFrame:(CGRect)frame;

@end




@interface UIImage(OrientationAdditions)

- (UIImage*)imageWithImageDataMatchingOrientation;

@end
