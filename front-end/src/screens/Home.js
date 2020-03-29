import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withNavigation } from 'react-navigation';

import { actions } from '../actions/Home';
import Bloco from '../components/Bloco';
import Produtos from '../components/Produtos';

class Home extends React.Component {
  static navigationOptions = {
    labelStyle: { color: '#ffffff' },
  };

  componentWillMount() {
    const { categoria, listProductsAction, navigation } = this.props;

    navigation.addListener('didFocus', () => listProductsAction(categoria));
  }

  render() {
    const { addToCartAction, changeQuantityAction, stateHome } = this.props;

    return (
      <Bloco>
        <Produtos
          produtos={stateHome.produtosFiltrados}
          changeQuantityAction={changeQuantityAction}
          addToCartAction={addToCartAction}
        />
      </Bloco>
    );
  }
}

const mapActionsToProps = dispatch => bindActionCreators(actions, dispatch);

const select = state => ({
  stateHome: state.Home,
});

export default connect(select, mapActionsToProps)(withNavigation(Home));
