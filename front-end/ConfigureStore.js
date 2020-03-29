import { createStore, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';

import reducers from './src/reducers';

const configure = () => createStore(reducers, applyMiddleware(thunk));

export default configure;
