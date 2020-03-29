import {
  Text,
  StyleSheet,
  Image,
  TouchableOpacity,
  Dimensions,
} from 'react-native';
import Icon from 'react-native-vector-icons/FontAwesome';
import React from 'react';

import Bloco from './Bloco';
import { Color } from '../constants/Theme';

const { width: WIDTH } = Dimensions.get('window');

const Styles = StyleSheet.create({
  header: {
    marginRight: 10,
    justifyContent: 'space-between',
    flexDirection: 'row',
  },
  image: {
    width: 90,
    height: 90,
  },
  actions: {
    marginRight: 10,
    justifyContent: 'space-between',
    flexDirection: 'row',
    bottom: 0,
    alignItems: 'center',
    height: 70,
  },
  card: {
    backgroundColor: '#fff',
    height: 100,
    borderRadius: 5,
    marginTop: 5,
    marginLeft: 5,
    marginRight: 5,
    flexDirection: 'row',
  },
  price: {
    fontSize: 17,
    color: Color.main,
    fontWeight: 'bold',
  },
  inputQuantidade: {
    marginLeft: 20,
    marginRight: 20,
    textAlign: 'center',
  },
  btnCarrinho: {
    backgroundColor: Color.main,
    borderRadius: 10,
    height: 30,
    justifyContent: 'space-between',
    alignItems: 'center',
    flexDirection: 'row',
  },
});

const CardTitle = ({ nome, preco }) => {
  return (
    <Bloco style={Styles.header}>
      <Text
        numberOfLines={2}
        style={{
          marginTop: 5,
          width: WIDTH * 0.4,
          height: 40,
          fontSize: 15,
          fontWeight: 'bold',
        }}
      >
        {nome}
      </Text>
      <Text style={Styles.price}>{`R$ ${preco?.toFixed(2)} `}</Text>
    </Bloco>
  );
};

const CardActions = ({ addToCartAction, changeQuantityAction, produto }) => (
  <Bloco style={Styles.actions}>
    <Bloco style={{ flexDirection: 'row', marginBottom: 5 }}>
      <TouchableOpacity
        style={{ marginLeft: 5 }}
        onPress={() => changeQuantityAction(produto.id, produto.quantidade - 1)}
      >
        <Icon size={25} name="minus" />
      </TouchableOpacity>
      <Text style={Styles.inputQuantidade}>
        {produto.quantidade && produto.quantidade.toString().padStart(2, '0')}
      </Text>
      <TouchableOpacity
        style={{ marginRight: 5 }}
        onPress={() => changeQuantityAction(produto.id, produto.quantidade + 1)}
      >
        <Icon size={25} name="plus" />
      </TouchableOpacity>
    </Bloco>
    <Bloco style={{ flexDirection: 'row', marginBottom: 5 }}>
      <TouchableOpacity
        style={Styles.btnCarrinho}
        onPress={() => addToCartAction(produto)}
      >
        <Icon
          size={15}
          style={{ marginLeft: 10 }}
          color={Color.exception}
          name="plus-circle"
        />
        <Text
          style={{
            color: Color.exception,
            fontWeight: '600',
            marginRight: 10,
            marginLeft: 10,
          }}
        >
          Adicionar
        </Text>
      </TouchableOpacity>
    </Bloco>
  </Bloco>
);

export default function Card({
  addToCartAction,
  changeQuantityAction,
  produto,
}) {
  return (
    <Bloco style={Styles.card}>
      <Image
        style={Styles.image}
        source={{ uri: `data:image/png;base64,${produto.image}` }}
      />
      <Bloco>
        <CardTitle nome={produto.nome} preco={produto.preco} />
        <CardActions
          addToCartAction={addToCartAction}
          changeQuantityAction={changeQuantityAction}
          produto={produto}
        />
      </Bloco>
    </Bloco>
  );
}
