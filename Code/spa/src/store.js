import { createStore, } from "redux";
import { loadUser } from "redux-oidc";
import reducer from "./reducers";
import userManager from "./userManager";

const initialState = {};

const store = createStore(reducer, initialState);
loadUser(store, userManager);

export default store;