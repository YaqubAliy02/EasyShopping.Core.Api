import 'package:flutter/material.dart';

class AppResponsive {
  static late double screenHeight;
  static late double screenWidth;

  static void init(BuildContext context) {
    final size = MediaQuery.of(context).size;
    screenHeight = size.height;
    screenWidth = size.width;
  }

  static double height(double number) => (number / screenHeight) * screenHeight;

  static double width(double number) => (number / screenWidth) * screenWidth;
}