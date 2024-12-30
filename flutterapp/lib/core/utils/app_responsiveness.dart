import 'package:flutter/material.dart';

class AppResponsive {
  static late double screenHeight;
  static late double screenWidth;

  static void init(BuildContext context) {
    final size = MediaQuery.of(context).size;
    screenHeight = size.height;
    screenWidth = size.width;
  }

  static double height(double percentage) => screenHeight * percentage;

  static double width(double percentage) => screenWidth * percentage;
}