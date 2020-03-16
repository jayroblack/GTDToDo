import { GET_OR_CRTEATE_USER , LOAD_LABELS, LOAD_PROJECTS } from './types';
import { GetOrCreateUser, GetToDos } from '../api/users';
import _ from 'lodash';

export const GetorCreateUser = (token, data) => {

    return async (dispatch) => {
        const response = await GetOrCreateUser(token, data);
        dispatch(
        {
            type: GET_OR_CRTEATE_USER,
            payload : response
        });
        dispatch(LoadLabelsAndProjectsForUser(token));
    }
}

export const LoadLabelsAndProjectsForUser = (token) => {

    return async (dispatch) => {
        const response = await GetToDos(token);

        if( response && response.data){
            const labels = _.mapKeys(response.data.labels, 'id');
            const inboxProject = response.data.projects.find( item => item.name === 'Inbox');
            const projectsWithoutInbox = response.data.projects.filter( item => item.name !== 'Inbox' );
            const projects = { Inbox: inboxProject, Data: _.mapKeys(projectsWithoutInbox, 'id') } ;

            dispatch({
                type: LOAD_LABELS,
                payload: labels
            });

            dispatch({
                type: LOAD_PROJECTS,
                payload: projects
            });

        }
    }
}