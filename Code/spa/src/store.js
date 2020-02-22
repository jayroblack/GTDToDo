import { createStore } from "redux";
import { loadUser } from "redux-oidc";
import reducer from "./reducers";
import userManager from "./userManager";
import { devToolsEnhancer } from 'redux-devtools-extension';

const initialState = {};

const store = createStore(reducer, initialState, devToolsEnhancer());
loadUser(store, userManager);

export default store;