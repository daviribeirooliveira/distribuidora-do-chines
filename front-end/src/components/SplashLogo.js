/* eslint-disable react/prefer-stateless-function */
import { Image } from 'react-native';
import React from 'react';

import images from '../constants/Images';

import Bloco from './Bloco';

export default class SplashLogo extends React.Component {
  render() {
    return (
      <Bloco center middle>
        <Image source={images.icon} />
      </Bloco>
    );
  }
}
