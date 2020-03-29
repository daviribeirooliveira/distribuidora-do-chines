/* eslint-disable react/prefer-stateless-function */
import { Dimensions, StyleSheet, TextInput, View } from 'react-native';
import React, { Component } from 'react';

import { Color, Text } from '../constants/Theme';

const { width: WIDTH } = Dimensions.get('window');

const styles = StyleSheet.create({
  Input: {
    backgroundColor: 'rgba(241, 196, 15,0.4)',
    borderRadius: 4,
    fontSize: Text.medium,
    height: 40,
    marginTop: 5,
    padding: 5,
    width: 120,
  },
});

export default class Input extends Component {
  render() {
    const {
      label,
      password,
      type,
      width,
      background,
      style,
      containerStyle,
      value,
      ...props
    } = this.props;

    const inputStyle = [
      styles.Input,
      width && { width: WIDTH * width },
      background && { backgroundColor: background },
      style,
    ];

    return (
      <View style={containerStyle}>
        <TextInput
          secureTextEntry={password}
          placeholder={` ${label}`}
          placeholderTextColor={Color.placeholder}
          autoCapitalize="none"
          autoCorrect={false}
          keyboardType={type}
          style={inputStyle}
          {...props}
        />
      </View>
    );
  }
}
