import 'package:riverpod/riverpod.dart';

class IsCustomer extends StateNotifier<bool>{
  IsCustomer() : super(true);

  void changeTheValue() {
    state = !state;
  }

} 

final isCustomerProvider = StateNotifierProvider<IsCustomer, bool>((ref) => IsCustomer());
