import { AsyncStorage } from 'react-native';
import React from 'react';

import SplashLogo from '../components/SplashLogo';

export default class SplashScreen extends React.Component {
  componentDidMount() {
    this.verificarAuth();
  }

  async verificarAuth() {
    const { navigation } = this.props;

    if (await AsyncStorage.getItem('auth')) {
      navigation.navigate('MainScreen');
    } else {
      navigation.navigate('Login');
    }
  }

  render() {
    return <SplashLogo />;
  }
}
