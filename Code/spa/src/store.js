import { createStore, compose, applyMiddleware } from "redux";
import { loadUser } from "redux-oidc";
import reducer from "./reducers";
import userManager from "./userManager";
import { devToolsEnhancer } from 'redux-devtools-extension';
import createOidcMiddleware from 'redux-oidc';

const oidcMiddleware = createOidcMiddleware(userManager);
const initialState = {};

const loggerMiddleware = store => next => action => {
    console.log("Action type:", action.type);
    console.log("Action payload:", action.payload);
    console.log("State before:", store.getState());
    next(action);
    console.log("State after:", store.getState());
  };

const createStoreWithMiddleware = compose(
    applyMiddleware(loggerMiddleware, oidcMiddleware)
  )(createStore);

const store = createStoreWithMiddleware(reducer, initialState, devToolsEnhancer());
loadUser(store, userManager);

export default store;