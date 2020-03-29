import { combineReducers } from 'redux';

import Alert from './Alert';
import Checkout from './Checkout';
import Home from './Home';

const Root = combineReducers({
  Alert,
  Checkout,
  Home,
});

export default Root;
