import React from 'react';
import { connect } from 'react-redux';
import { withStyles } from '@material-ui/core/styles';
import { Dialog, DialogTitle, DialogContent, DialogContentText, TextField, DialogActions, Button } from '@material-ui/core'
import { CloseNewProjectDialog, InvalidateNewProjectDialog, SaveNewProjectDialog } from '../actions/newProjectDialog';

const useStyles = theme => ({

});

class NewProjectDialog extends React.Component {
    
    handleDismiss = (cancelled) => {
        this.props.dispatch(CloseNewProjectDialog(cancelled));
    };

    handleSave = () => {
        //How do I get the value of the project? 
        //If length == 0 then 1) Make field Red - show error message 2) Disable the Save Button
        //If Length > 0 however the Project Name already exists 1) Make field Red - show error message 2) Disable the Save Button
        
        //When saving, - change to spinner - save and cancel are disabled
        //When Save Returns Succeesful - Close Dialog - show Snack Bar Success.
        //When Save Returns Failed - 1) Make Field Red - show error message - cancel and save are enabled again.
        //this.props.dispatch(SaveNewProjectDialog());
    };

    render() {
        return (
            <Dialog open={this.props.newProjectDialog.status !== 'closed'} onClose={() => this.handleDismiss(false)} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">Add New Project</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Enter the new project name and click save.
                    </DialogContentText>
                    <TextField
                        autoFocus
                        required 
                        margin="dense"
                        id="project"
                        label="Project Name"
                        fullWidth
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={ ()=> this.handleDismiss(true)} color="primary">
                        Cancel
                    </Button>
                    <Button onClick={this.handleSave} color="primary">
                        Save
                    </Button>
                </DialogActions>
            </Dialog>
        );
    }
}

function mapStateToProps(state) {
    return { projects: state.labelsAndProjects, newProjectDialog: state.newProjectDialog };
}

function mapDispatchToProps(dispatch) {
    return {
      dispatch
    };
  }

export default connect(mapStateToProps, mapDispatchToProps)(withStyles(useStyles)(NewProjectDialog));