import React from 'react';
import { connect } from 'react-redux';
import { withStyles } from '@material-ui/core/styles';
import { CloseProjectDialog, SavingProjectDialog, UpdateNewProjectDialog } from '../actions/projectDialog';
import ProjectFormDialog from './ProjectFormDialog'

const useStyles = theme => ({

});

class EditProjectDialog extends React.Component {

    onSubmit = (formValues) => {
        this.props.dispatch(SavingProjectDialog());
        this.timer = setTimeout(() =>{
            const data = { id:this.props.projectDialogState.id, name: formValues.name, versionNumber: this.props.projectDialogState.versionNumber };
            this.props.dispatch(UpdateNewProjectDialog(this.props.userProfile.access_token, data));
        } , 2000)
    };

    componentDidUpdateCallback = (prevProps, currentProps) => {
        if( prevProps.projectDialogState.status === 'saving' && 
        currentProps.projectDialogState.status === 'saveSucceeded' && 
        prevProps.projectDialogState.isNew === false ){
            currentProps.enqueueSnackbar('Project Updated.', { key: "ProjectUpdated", persist: false, variant: 'success' });
            currentProps.dispatch(CloseProjectDialog(true));
        }
    }

    isOpen = () => {
        return this.props.projectDialogState.status !== 'closed' && !this.props.projectDialogState.isNew;
    }

    render() {
        
        return (
            <ProjectFormDialog 
                title="Edit Project" 
                description="Project names must be unique." 
                onSubmit={ this.onSubmit } 
                componentDidUpdateCallback={ this.componentDidUpdateCallback } 
                isOpen={ this.isOpen() }
            />
        );
    }
}

EditProjectDialog = connect(
    state => {
      return { 
          projects: state.projects, 
          projectDialogState: state.projectDialog,
          myForm: state.form.projectDialog,
          userProfile: state.userProfile
        };
    }, 
    dispatch => {
        return { dispatch }
    }
  )(EditProjectDialog)

export default (withStyles(useStyles))( EditProjectDialog);