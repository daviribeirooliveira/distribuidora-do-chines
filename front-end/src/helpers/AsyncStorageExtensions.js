import { AsyncStorage } from 'react-native';

export const AsyncStorageSetObject = async (itemName, object, callback) =>
  AsyncStorage.setItem(itemName, JSON.stringify(object), callback);

export const AsyncStorageGetObject = async (itemName, callback) =>
  JSON.parse(await AsyncStorage.getItem(itemName, callback));
