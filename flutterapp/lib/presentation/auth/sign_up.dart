import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:flutterapp/core/common/constants/app_colors.dart';
import 'package:flutterapp/core/common/constants/font_names.dart';
import 'package:flutterapp/core/utils/app_responsiveness.dart';
import 'package:flutterapp/presentation/auth/providers/is_customer.dart';
import 'package:flutterapp/presentation/auth/providers/is_visible.dart';

class SignUpScreen extends StatefulWidget {
  const SignUpScreen({super.key});

  @override
  State<SignUpScreen> createState() => _SignUpScreenState();
}

class _SignUpScreenState extends State<SignUpScreen> {
  TextEditingController usernameController = TextEditingController();

  TextEditingController emailController = TextEditingController();

  TextEditingController passwordController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Consumer(
      builder: (context, ref, child) => Scaffold(
        resizeToAvoidBottomInset: false,
        body: Container(
          width: double.infinity,
          padding: EdgeInsets.symmetric(
            horizontal: AppResponsive.width(19),
            // vertical: AppResponsive.height(115),
          ),
          decoration: BoxDecoration(
            gradient: LinearGradient(
              begin: Alignment.topCenter,
              colors: [
                AppColors.lightOrange.withValues(alpha: 0.45),
                AppColors.lightOrange.withValues(alpha: 0.45),
                AppColors.lightPurple.withValues(alpha: 0.45),
              ],
            ),
          ),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Container(
                width: double.infinity,
                padding: EdgeInsets.symmetric(
                  vertical: AppResponsive.height(25),
                  horizontal: AppResponsive.width(19),
                ),
                decoration: BoxDecoration(
                  color: AppColors.white.withValues(alpha: 0.7),
                  borderRadius: BorderRadius.circular(AppResponsive.width(15)),
                ),
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Text(
                      "Sign Up",
                      style: TextStyle(
                        fontSize: AppResponsive.width(32),
                        fontWeight: FontWeight.bold,
                        fontFamily: FontNames.interBold,
                      ),
                    ),
                    SizedBox(
                      height: AppResponsive.height(7),
                    ),
                    Text(
                      "Create an account to continue!",
                      style: TextStyle(
                        fontSize: AppResponsive.width(12),
                        fontFamily: FontNames.interMedium,
                        color: AppColors.black.withValues(alpha: 0.5),
                      ),
                    ),
                    SizedBox(
                      height: AppResponsive.height(13),
                    ),
                    customTextFields("Username", usernameController),
                    customTextFields("Email Address", emailController),
                    customTextFieldsPassword(
                        "Password",
                        passwordController,
                        ref.watch(isVisibleProvider),
                        ref.read(isVisibleProvider.notifier).changeTheValue),
                    Row(
                      children: [
                        Text(
                          "Are you a customer?",
                          style: TextStyle(
                            fontSize: AppResponsive.width(14),
                            fontFamily: FontNames.interMedium,
                            color: AppColors.black,
                            fontWeight: FontWeight.w600,
                          ),
                        ),
                        Theme(
                          data: Theme.of(context).copyWith(
                            radioTheme: RadioThemeData(
                              fillColor: WidgetStatePropertyAll(
                                  AppColors.weirdBlue), // Active color
                            ),
                          ),
                          child: Radio(
                            value: true,
                            groupValue: ref.watch(isCustomerProvider),
                            toggleable: true,
                            onChanged: (value) {
                              setState(() {
                                ref
                                    .read(isCustomerProvider.notifier)
                                    .changeTheValue();
                              });
                            },
                          ),
                        ),
                      ],
                    ),
                    SizedBox(
                      height: AppResponsive.height(13),
                    ),
                    customButton("Register", () {}),
                    SizedBox(
                      height: AppResponsive.height(25),
                    ),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Text(
                          "Already have an account?  ",
                          style: TextStyle(
                            fontSize: AppResponsive.width(12),
                            fontFamily: FontNames.interMedium,
                            color: AppColors.black.withValues(alpha: 0.5),
                            fontWeight: FontWeight.w600,
                          ),
                        ),
                        GestureDetector(
                          onTap: () {},
                          child: Text(
                            "Login",
                            style: TextStyle(
                              fontSize: AppResponsive.width(12),
                              fontFamily: FontNames.interMedium,
                              fontWeight: FontWeight.w700,
                              color: AppColors.blue,
                            ),
                          ),
                        )
                      ],
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Container customButton(String text, VoidCallback onPressed) {
    return Container(
      height: AppResponsive.height(48),
      width: double.infinity,
      decoration: BoxDecoration(
        color: AppColors.weirdBlue,
        borderRadius: BorderRadius.circular(
          AppResponsive.width(9),
        ),
      ),
      child: TextButton(
        onPressed: onPressed,
        child: Text(
          text,
          style: TextStyle(
            fontSize: AppResponsive.width(14),
            fontFamily: FontNames.interMedium,
            color: AppColors.white,
          ),
        ),
      ),
    );
  }

  Container customTextFields(String text, TextEditingController controller) {
    return Container(
      height: AppResponsive.height(46),
      width: double.infinity,
      alignment: Alignment.center,
      padding: EdgeInsets.only(
        left: AppResponsive.width(15),
      ),
      margin: EdgeInsets.only(
        bottom: AppResponsive.height(11),
      ),
      decoration: BoxDecoration(
        color: AppColors.white,
        borderRadius: BorderRadius.circular(
          AppResponsive.width(10),
        ),
        boxShadow: [
          BoxShadow(
              color: AppColors.black.withValues(alpha: 0.07),
              blurRadius: 5,
              spreadRadius: 0.3,
              offset: Offset(0, AppResponsive.width(2))),
        ],
      ),
      child: TextField(
        controller: controller,
        style: TextStyle(
          fontSize: AppResponsive.width(14),
          fontFamily: FontNames.interMedium,
          fontWeight: FontWeight.w600,
          color: AppColors.black,
          decoration: TextDecoration.none,
          decorationThickness: 0,
        ),
        decoration: InputDecoration(
          hintText: text,
          hintStyle: TextStyle(
            fontSize: AppResponsive.width(14),
            fontFamily: FontNames.interMedium,
            fontWeight: FontWeight.w600,
            color: AppColors.black.withValues(alpha: 0.5),
          ),
          border: const UnderlineInputBorder(
            borderSide: BorderSide.none,
          ),
        ),
      ),
    );
  }

  Container customTextFieldsPassword(
      String text,
      TextEditingController controller,
      bool obscureText,
      VoidCallback onPressed) {
    return Container(
      height: AppResponsive.height(46),
      width: double.infinity,
      alignment: Alignment.center,
      padding: EdgeInsets.only(
        left: AppResponsive.width(15),
      ),
      margin: EdgeInsets.only(
        bottom: AppResponsive.height(11),
      ),
      decoration: BoxDecoration(
        color: AppColors.white,
        borderRadius: BorderRadius.circular(
          AppResponsive.width(10),
        ),
        boxShadow: [
          BoxShadow(
              color: AppColors.black.withValues(alpha: 0.07),
              blurRadius: 5,
              spreadRadius: 0.3,
              offset: Offset(0, AppResponsive.width(2))),
        ],
      ),
      child: Row(
        children: [
          Expanded(
            child: TextField(
              controller: controller,
              style: TextStyle(
                fontSize: AppResponsive.width(14),
                fontFamily: FontNames.interMedium,
                fontWeight: FontWeight.w600,
                color: AppColors.black,
                decoration: TextDecoration.none,
                decorationThickness: 0,
              ),
              obscureText: obscureText,
              decoration: InputDecoration(
                hintText: text,
                hintStyle: TextStyle(
                  fontSize: AppResponsive.width(14),
                  fontFamily: FontNames.interMedium,
                  fontWeight: FontWeight.w600,
                  color: AppColors.black.withValues(alpha: 0.5),
                ),
                border: const UnderlineInputBorder(
                  borderSide: BorderSide.none,
                ),
              ),
            ),
          ),
          IconButton(
            onPressed: onPressed,
            icon: Icon(
              obscureText ? Icons.remove_red_eye : Icons.visibility_off,
              size: AppResponsive.width(19),
              color: AppColors.black.withValues(alpha: 0.7),
            ),
            color: AppColors.black,
          ),
        ],
      ),
    );
  }
}
