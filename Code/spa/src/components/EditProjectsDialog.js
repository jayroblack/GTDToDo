import React from 'react';
import { connect } from 'react-redux';
import { withStyles } from '@material-ui/core/styles';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, LinearProgress } from '@material-ui/core'
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import { CloseEditProjectsDialog, PendingEditProjectsDialog, DeleteProjectEditProjectsDialog } from '../actions/editProjectsDialog';
import { OpenSobrietyPrompt } from '../actions/sobrietyPromptDialog';
import { withSnackbar } from 'notistack';
import SobrietyPromptDialog from './SobrietyPromptDialog';

const useStyles = theme => ({

});

class EditProjectsDialog extends React.Component {
    //TODO: Add Sobriety Prompt before Deleting Project. 
    //TODO: Show Error Message if Delete Fails. Snackbar?
    //TODO: Figure out how to refactor New Project dialog for Edit Project dialog. 
    constructor(props) {
        super(props);
        this.timer = null;
    }

    handleDismiss = (cancelled) => {
        this.props.dispatch(CloseEditProjectsDialog(cancelled));
    };

    handleDelete = (projectId) => {
        this.props.dispatch(OpenSobrietyPrompt("Delete Project", "Are you sure you want to delete this project?", projectId))
    }

    componentDidUpdate(prevProps) {
        
        if (prevProps.editProjectsDialogState.status === 'pending' &&
            this.props.editProjectsDialogState.status === 'succeeded') {
            this.props.enqueueSnackbar('Project Deleted.', { key: "Project Deleted", persist: false, variant: 'success' });
        }

        if( prevProps.sobrietyPromptDialog.status === 'open' && 
            this.props.sobrietyPromptDialog.status === 'closed' && 
            this.props.sobrietyPromptDialog.result === 'yes'){

                this.props.dispatch(PendingEditProjectsDialog());
                this.timer = setTimeout(() => {
                    this.props.dispatch(DeleteProjectEditProjectsDialog(this.props.userProfile.access_token, this.props.sobrietyPromptDialog.data));
                }, 2000)
        }
    }

    componentWillUnmount() {
        clearTimeout(this.timer);
    }

    render() {
        let disabledOptions = {};

        if (this.props.editProjectsDialogState.status === 'pending') {
            disabledOptions = { disabled: true };
        }
        else {
            disabledOptions = {};
        }

        return (
            <React.Fragment>
                <SobrietyPromptDialog />
                <Dialog open={this.props.editProjectsDialogState.status !== 'closed'}
                    onClose={() => this.handleDismiss(false)}
                    fullWidth={true}
                    maxWidth="sm"
                >
                    <DialogTitle id="form-dialog-title">Edit Projects</DialogTitle>
                    <DialogContent>
                        <TableContainer>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell>Project Name</TableCell>
                                        <TableCell align="right"></TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {this.props.projects.map(project => (
                                        <TableRow key={project.id}>
                                            <TableCell component="th" scope="row">
                                                {project.name}
                                            </TableCell>
                                            <TableCell align="right">
                                                <Button variant="contained" color="primary" {...disabledOptions}>
                                                    Edit
                                    </Button>
                                                <span width="200">&nbsp;&nbsp;&nbsp;</span>
                                                <Button variant="contained"
                                                    color="secondary"
                                                    onClick={() => this.handleDelete(project.id)}
                                                    {...disabledOptions}>
                                                    Delete
                                    </Button>
                                            </TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </TableContainer>
                    </DialogContent>
                    <DialogActions>
                        <Button variant="contained" onClick={() => this.handleDismiss(true)} color="primary" {...disabledOptions}>
                            Close
                    </Button>
                    </DialogActions>
                    {this.props.editProjectsDialogState.status === 'pending' && <LinearProgress />}
                </Dialog>
            </React.Fragment>
        );
    }
}

EditProjectsDialog = connect(
    state => {
        var projectData = [];
        if (state.projects && state.projects.Data) {
            projectData = Object.values(state.projects.Data);
        }
        return {
            projects: projectData,
            editProjectsDialogState: state.editProjectsDialog,
            sobrietyPromptDialog: state.sobrietyPromptDialog,
            userProfile: state.userProfile
        };
    },
    dispatch => {
        return { dispatch }
    }
)(EditProjectsDialog)

export default (withStyles(useStyles))(withSnackbar(EditProjectsDialog));