import 'package:riverpod/riverpod.dart';

class IsVisible extends StateNotifier<bool> {
  IsVisible() : super(false);

  void changeTheValue() {
    state = !state;
  }

}

final isVisibleProvider = StateNotifierProvider<IsVisible, bool>((ref) => IsVisible());