import 'package:flutter/material.dart';
import 'package:flutterapp/core/utils/app_responsiveness.dart';
import 'package:flutterapp/presentation/splash/splash_screen.dart';

void main() {
  runApp(
    const MyApp(),
  );
}

class MyApp extends StatelessWidget {
  const MyApp({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    AppResponsive.init(context);
    return const MaterialApp(
      debugShowCheckedModeBanner: false,
      home: SplashScreen(),
    );
  }
}
