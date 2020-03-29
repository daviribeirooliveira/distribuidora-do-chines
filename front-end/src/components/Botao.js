import { Dimensions, StyleSheet, TouchableOpacity } from 'react-native';
import React from 'react';

import { Color } from '../constants/Theme';
import Texto from './Texto';

const { width: WIDTH } = Dimensions.get('window');

const Styles = StyleSheet.create({
  botao: {
    alignItems: 'center',
    backgroundColor: Color.main,
    borderRadius: 4,
    height: 35,
    justifyContent: 'center',
  },
});

export default function Botao(props) {
  const { children, width } = props;

  const buttonStyle = [Styles.botao, width && { width: WIDTH * width }];

  return (
    <TouchableOpacity {...props} style={buttonStyle}>
      {typeof children === 'string' ? (
        <Texto color={Color.secundary}>{children}</Texto>
      ) : (
        children
      )}
    </TouchableOpacity>
  );
}
