import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { FlatList } from 'react-native-gesture-handler';

import { actions } from '../actions/Home';
import { Color } from '../constants/Theme';
import Bloco from './Bloco';
import Card from './Card';
import Input from './Input';

function Produtos(props) {
  const {
    addToCartAction,
    changeQuantityAction,
    filterProductsAction,
    produtos,
  } = props;

  return (
    <Bloco center>
      <Input
        containerStyle={{
          backgroundColor: Color.main,
          padding: 5,
          width: '100%',
          alignItems: 'center',
        }}
        style={{ textAlign: 'center', borderRadius: 10, height: 30 }}
        width={0.9}
        placeholderTextColor={Color.text}
        background={Color.exception}
        placeholder="O que você está buscando?"
        underlineColorAndroid="transparent"
        autoCapitalize="none"
        onChangeText={filterProductsAction}
      />
      <Bloco
        style={{
          marginTop: 5,
          marginBottom: 50,
          width: '100%',
          backgroundColor: '#f1f2f6',
        }}
      >
        <FlatList
          keyExtractor={item => item.id.toString()}
          data={produtos || null}
          renderItem={({ item }) => (
            <Card
              produto={item}
              changeQuantityAction={changeQuantityAction}
              addToCartAction={addToCartAction}
            />
          )}
        />
      </Bloco>
    </Bloco>
  );
}

const mapActionsToProps = dispatch => bindActionCreators(actions, dispatch);

export default connect(null, mapActionsToProps)(Produtos);
