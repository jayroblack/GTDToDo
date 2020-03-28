import { combineReducers } from 'redux';
import { reducer as oidcReducer } from 'redux-oidc';
import { reducer as formReducer } from 'redux-form';
import {GET_OR_CRTEATE_USER , LOAD_LABELS } from '../actions/types';
import { USER_FOUND } from 'redux-oidc/src/constants';
import NameFormDialog from './nameFormDialog';
import EditProjectDialog from './editProjectsDialog'
import SobrietyPromptDialog from './sobrietyPromptDialog'
import projectReducer from './projects';

const GetorCreateUserReducer = (user = null, action) => {
  if( action.type === GET_OR_CRTEATE_USER ){
    return action.payload;
  }
  return user;
}

const LoadLabels = (labels = { }, action) => {
  if( action.type === LOAD_LABELS ){
    return action.payload;
  }
  return labels;
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
      form: formReducer,
      oidc: oidcReducer,
      userData: GetorCreateUserReducer,
      labels: LoadLabels,
      projects: projectReducer,
      userProfile: UserProfileReducer,
      nameFormDialog: NameFormDialog,
      editProjectsDialog: EditProjectDialog,
      sobrietyPromptDialog: SobrietyPromptDialog
    }
  );

export default reducer;