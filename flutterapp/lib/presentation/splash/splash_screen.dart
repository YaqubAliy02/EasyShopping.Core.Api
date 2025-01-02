import 'package:flutter/material.dart';
import 'package:flutterapp/core/common/constants/app_colors.dart';
import 'package:flutterapp/core/common/constants/font_names.dart';
import 'package:flutterapp/core/common/constants/image_paths.dart';
import 'package:flutterapp/core/utils/app_responsiveness.dart';

class SplashScreen extends StatelessWidget {
  const SplashScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.white,
      body: SizedBox(
        width: double.infinity,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Container(
              height: AppResponsive.height(134),
              width: AppResponsive.width(134),
              decoration: BoxDecoration(
                color: AppColors.white,
                shape: BoxShape.circle,
                boxShadow: [
                  BoxShadow(
                    color: AppColors.black.withValues(alpha: 0.3),
                    blurRadius: 5,
                    spreadRadius: 0.3,
                  ),
                ],
              ),
              padding: EdgeInsets.all(AppResponsive.width(21)),
              child: Image(
                image: AssetImage(ImagePaths.bag),
              ),
            ),
            SizedBox(
              height: AppResponsive.height(10),
            ),
            Text(
              "Shop",
              style: TextStyle(
                fontSize: AppResponsive.width(52),
                fontFamily: FontNames.ralewayBold,
                fontWeight: FontWeight.bold,
              ),
            ),
            SizedBox(
              height: AppResponsive.height(10),
            ),
            Text(
              textAlign: TextAlign.center,
              "Beautiful eCommerce UI Kit \nfor your online store",
              style: TextStyle(
                fontSize: AppResponsive.width(19),
                fontFamily: FontNames.nunitoSansLight,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
