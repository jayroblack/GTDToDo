import React from 'react';
import { connect } from 'react-redux';
import { withStyles } from '@material-ui/core/styles';
import { CloseNameFormDialog, SavingNameFormDialog, SaveNameFormDialog } from '../actions/nameFormDialog';
import NameFormDialog from './NameFormDialog'
import { CreateProject } from '../api/projects';
import { ADD_PROJECT } from '../actions/types';
const useStyles = theme => ({

});

class NewProjectDialog extends React.Component {

    onSubmit = (formValues) => {
        this.props.dispatch(SavingNameFormDialog());
        this.timer = setTimeout( async () =>{
            const data = { name: formValues.name };

            const response = await CreateProject(this.props.userProfile.access_token, data);
            const objectToDispatch = 
            {
                type: ADD_PROJECT,
                payload: response.data
            };

            this.props.dispatch(SaveNameFormDialog(response, objectToDispatch));

        } , 2000)
    };

    componentDidUpdateCallback = (prevProps, currentProps) => {
        if( prevProps.nameFormDialog.status === 'saving' && 
            currentProps.nameFormDialog.status === 'saveSucceeded' && 
            prevProps.nameFormDialog.isNew === true){
            
            currentProps.enqueueSnackbar('New Project Saved.', { key: "NewProjectSaveSucceeded", persist: false, variant: 'success' });
            currentProps.dispatch(CloseNameFormDialog(true));
        }
    }

    isOpen = () => {
        return this.props.nameFormDialog.entity === "Project" && 
            this.props.nameFormDialog.status !== 'closed' && 
            this.props.nameFormDialog.isNew === true ;
    }

    render() {
        
        return (
            <NameFormDialog 
                title="Create New Project" 
                entity="Project" 
                description="Enter the name of your new project." 
                onSubmit={ this.onSubmit } 
                componentDidUpdateCallback={ this.componentDidUpdateCallback } 
                isOpen={ this.isOpen() }
            />
        );
    }
}

NewProjectDialog = connect(
    state => {
      return { 
          nameFormDialog: state.nameFormDialog,
          userProfile: state.userProfile
        };
    }, 
    dispatch => {
        return { dispatch }
    }
  )(NewProjectDialog)

export default (withStyles(useStyles))( NewProjectDialog);