import { combineReducers } from 'redux';
import { reducer as oidcReducer } from 'redux-oidc';

const reducer = combineReducers(
    {
      oidc: oidcReducer
    }
  );

export default reducer;