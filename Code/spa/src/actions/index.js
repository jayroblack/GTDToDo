import { GET_OR_CRTEATE_USER , LOAD_LABELS_AND_PROJECTS } from './types';
import { GetOrCreateUser, GetToDos } from '../api/users';

export const GetorCreateUser = (token, data) => {

    return async (dispatch) => {
        const response = await GetOrCreateUser(token, data);
        dispatch(
        {
            type: GET_OR_CRTEATE_USER,
            payload : response
        });
    }
}

export const LoadLabelsAndProjectsForUser = (token) => {

    return async (dispatch) => {
        const response = await GetToDos(token);
        dispatch(
        {
            type: LOAD_LABELS_AND_PROJECTS,
            payload: response
        });
    }
}