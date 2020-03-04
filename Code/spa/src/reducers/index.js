import { combineReducers } from 'redux';
import { reducer as oidcReducer } from 'redux-oidc';
import {GET_OR_CRTEATE_USER , LOAD_LABELS_AND_PROJECTS } from '../actions/types';
import { USER_FOUND } from 'redux-oidc/src/constants';

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

const UserProfileReducer = (userProfile = null, action) => {
  if( action.type === USER_FOUND ){
    const user = action.payload;
    const { given_name, family_name, email, sub } = user.profile;
    return Object.assign({}, { ...userProfile }, { access_token: user.access_token, given_name, family_name, email, userId: sub });
  }
  return userProfile;
}

const reducer = combineReducers(
    {
      oidc: oidcReducer,
      userData: GetorCreateUserReducer,
      labelsAndProjects: LoadLabelsAndProjectsReducer,
      userProfile: UserProfileReducer
    }
  );

export default reducer;