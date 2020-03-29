/* eslint-disable import/no-extraneous-dependencies */

import React from 'react';
import { Image } from 'react-native';
import Icon from 'react-native-vector-icons/FontAwesome';

import { Color } from '../constants/Theme';
import images from '../constants/Images';

import Bloco from '../components/Bloco';

const navigationOptions = ({ navigation }) => ({
  headerBackTitle: 'Voltar',
  headerStyle: {
    backgroundColor: Color.main,
    height: 50,
    shadowColor: 'transparent',
    elevation: 0,
    borderBottomWidth: 0,
  },
  headerTintColor: Color.secundary,
  headerTitle: (
    <Bloco row middle>
      <Image
        source={images.logo}
        style={{ width: 220, height: 45 }}
        resizeMode="contain"
      />
    </Bloco>
  ),
  headerLeft: (
    <Icon
      style={{ marginLeft: 10 }}
      name="bars"
      size={30}
      color={Color.secundary}
      onPress={() => {
        navigation.openDrawer();
      }}
    />
  ),
  headerRight: (
    <Icon
      style={{ marginRight: 10 }}
      name="shopping-cart"
      size={30}
      color={Color.secundary}
      onPress={() => {
        navigation.navigate('Checkout');
      }}
    />
  ),
});

export default navigationOptions;
