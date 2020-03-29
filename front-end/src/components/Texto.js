/* eslint-disable react/prefer-stateless-function */
import React, { Component } from 'react';
import { Text as BaseText, StyleSheet } from 'react-native';

import { Text as TextTheme } from '../constants/Theme';

const Styles = StyleSheet.create({
  base: {
    textAlignVertical: 'center',
    textAlign: 'center',
    fontFamily: 'Roboto',
  },
});

export default class Text extends Component {
  render() {
    const {
      children,
      color,
      size,
      h1,
      h2,
      h3,
      sublinhado,
      italico,
      style,
      ...props
    } = this.props;

    const TextStyle = [
      Styles.base,
      color && { color },
      size && { fontSize: size },
      h1 && { fontSize: TextTheme.h1 },
      h2 && { fontSize: TextTheme.h2 },
      h3 && { fontSize: TextTheme.h3 },
      sublinhado && { textDecorationLine: 'underline' },
      italico && { fontStyle: 'italic' },
      style,
    ];

    return (
      <BaseText {...props} style={TextStyle}>
        {children}
      </BaseText>
    );
  }
}
