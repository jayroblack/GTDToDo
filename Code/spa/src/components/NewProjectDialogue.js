import React from 'react';
import { connect } from 'react-redux';
import { withStyles } from '@material-ui/core/styles';

const useStyles = theme => ({

});

class NewProjectDialog extends React.Component {
    constructor(props) {
        super(props);
        this.state = { newProjectIsOpen: false };
    }

    handleClickOpen = () => {
        this.setState({ newProjectIsOpen: true })
    };

    handleClose = () => {
        this.setState({ newProjectIsOpen: false })
    };

    render() {
        <Dialog open={this.state.newProjectIsOpen} onClose={handleClose} aria-labelledby="form-dialog-title">
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
                <Button onClick={handleClose} color="primary">
                    Cancel
                </Button>
                <Button onClick={handleClose} color="primary">
                    Save
                </Button>
            </DialogActions>
        </Dialog>
    }
}

function mapStateToProps(state) {
    if (!state.LabelsAndProjects) {
        return { projects: null };
    }
    return { projects: state.LabelsAndProjects.data.projects };
}

export default connect(mapStateToProps)(withStyles(useStyles)(NewProjectDialog));