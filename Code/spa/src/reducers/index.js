import { combineReducers } from 'redux';
import { reducer as oidcReducer } from 'redux-oidc';
import {GET_OR_CRTEATE_USER , LOAD_LABELS_AND_PROJECTS } from '../actions/types';

const GetorCreateUserReducer = (user = null, action) => {
  if( action.type === GET_OR_CRTEATE_USER ){
    return action.payload;
  }
  return user;
}

const LoadLabelsAndProjectsReducer = (labelsAndProjects = null, action) => {
  if( action.type === LOAD_LABELS_AND_PROJECTS ){
    return action.payload;
  }
  return labelsAndProjects;
}

const reducer = combineReducers(
    {
      oidc: oidcReducer,
      GetorCreateUserReducer,
      LoadLabelsAndProjectsReducer
    }
  );

export default reducer;