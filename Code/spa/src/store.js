import { createStore, compose, applyMiddleware } from "redux";
import { loadUser } from "redux-oidc";
import reducer from "./reducers";
import userManager from "./userManager";
import { devToolsEnhancer } from 'redux-devtools-extension';
import createOidcMiddleware from 'redux-oidc';
import thunk from 'redux-thunk';

const oidcMiddleware = createOidcMiddleware(userManager);
const initialState = {};

const createStoreWithMiddleware = compose(
    applyMiddleware(thunk, oidcMiddleware)
  )(createStore);

const store = createStoreWithMiddleware(reducer, initialState, devToolsEnhancer());
loadUser(store, userManager);

export default store;