import React from 'react';
import { connect } from 'react-redux';
import { withStyles } from '@material-ui/core/styles';
import { CloseProjectDialog, SavingProjectDialog, SaveNewProjectDialog } from '../actions/projectDialog';
import NameFormDialog from './NameFormDialog'

const useStyles = theme => ({

});

class NewProjectDialog extends React.Component {

    onSubmit = (formValues) => {
        this.props.dispatch(SavingProjectDialog());
        this.timer = setTimeout(() =>{
            const data = { name: formValues.name };
            this.props.dispatch(SaveNewProjectDialog(this.props.userProfile.access_token, data));
        } , 2000)
    };

    componentDidUpdateCallback = (prevProps, currentProps) => {
        if( prevProps.projectDialogState.status === 'saving' && 
            currentProps.projectDialogState.status === 'saveSucceeded' && 
            prevProps.projectDialogState.isNew === true){
            
            currentProps.enqueueSnackbar('New Project Saved.', { key: "NewProjectSaveSucceeded", persist: false, variant: 'success' });
            currentProps.dispatch(CloseProjectDialog(true));
        }
    }

    isOpen = () => {
        return this.props.projectDialogState.status !== 'closed' && this.props.projectDialogState.isNew === true ;
    }

    render() {
        
        return (
            <NameFormDialog 
                title="Create New Project" 
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
          projects: state.projects, 
          projectDialogState: state.projectDialog,
          myForm: state.form.projectDialog,
          userProfile: state.userProfile
        };
    }, 
    dispatch => {
        return { dispatch }
    }
  )(NewProjectDialog)

export default (withStyles(useStyles))( NewProjectDialog);