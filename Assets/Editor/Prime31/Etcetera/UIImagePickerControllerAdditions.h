//
//  UIImagePickerController+UIImagePickerControllerAdditions.h
//  Unity-iPhone


#import <UIKit/UIKit.h>


@interface UIImagePickerController (Additions)

+ (void)obtainPermissionForMediaSourceType:(UIImagePickerControllerSourceType)sourceType withSuccessHandler:(void (^) ())successHandler andFailure:(void (^) ())failureHandler;

@end
