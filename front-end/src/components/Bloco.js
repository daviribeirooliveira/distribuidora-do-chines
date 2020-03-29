import React from 'react';
import { StyleSheet, Text, View } from 'react-native';

const baseStyles = StyleSheet.create({
  bloc: {
    flex: 1,
  },
  row: {
    flexDirection: 'row',
  },
});

function Bloco(props) {
  const { background, center, children, middle, row } = props;

  const styles = [
    baseStyles.bloc,
    background && { backgroundColor: background },
    center && { alignItems: 'center' },
    middle && { justifyContent: 'center' },
    row && baseStyles.row,
  ];

  return (
    <View style={styles} {...props}>
      {typeof children === 'string' ? <Text>{children}</Text> : children}
    </View>
  );
}

export default Bloco;
