import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Image, StyleSheet } from 'react-native';
import React from 'react';

import {
  Card,
  CardItem,
  Text,
  Button,
  Icon,
  Left,
  Body,
  Right,
} from 'native-base';

import { actions } from '../actions/Checkout';
import { Color } from '../constants/Theme';

const Styles = StyleSheet.create({
  cardImage: {
    height: 80,
    width: 80,
    flex: 0.3,
    marginLeft: 20,
    marginTop: 10,
  },
  cardPrice: {
    fontSize: 17,
    color: Color.main,
    fontWeight: 'bold',
  },
});

function CardCheckout(props) {
  const { produto, removeFromCartAction } = props;

  return (
    <Card>
      <CardItem cardBody>
        <Left>
          <Image
            source={{ uri: `data:image/png;base64,${produto.imagem}` }}
            style={Styles.cardImage}
          />
        </Left>
        <Body>
          <Text
            style={{ marginTop: 10, textAlign: 'center', fontWeight: 'bold' }}
          >
            {produto.nome} un.
          </Text>
        </Body>
        <Right />
      </CardItem>
      <CardItem>
        <Left>
          <Text>{produto.quantidade} un.</Text>
        </Left>
        <Body>
          <Button style={{ justifyContent: 'center' }} transparent>
            <Icon
              active
              style={{ fontSize: 30 }}
              name="trash"
              onPress={() => removeFromCartAction(produto.idProduto)}
            />
          </Button>
        </Body>
        <Right>
          <Text style={Styles.cardPrice}>{`R$ ${produto.valor.toFixed(
            2
          )}`}</Text>
        </Right>
      </CardItem>
    </Card>
  );
}

const mapActionsToProps = dispatch => bindActionCreators(actions, dispatch);

export default connect(null, mapActionsToProps)(CardCheckout);
