import React from "react";
import { connect } from "react-redux";
import { GetorCreateUser } from '../actions';
import { withStyles } from '@material-ui/core/styles';
import Drawer from '@material-ui/core/Drawer';
import List from '@material-ui/core/List';
import Typography from '@material-ui/core/Typography';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import ExpansionPanelActions from '@material-ui/core/ExpansionPanelActions';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import { withSnackbar } from 'notistack';
import Button from '@material-ui/core/Button';
import NewProjectDialog from './NewProjectDialog';
import EditProjectsDialog from './EditProjectsDialog';
import { OpenProjectDialog } from '../actions/projectDialog';
import { OpenEditProjectsDialog } from '../actions/editProjectsDialog'

const drawerWidth = 240;

const useStyles = theme => ({
  drawer: {
    width: drawerWidth,
    flexShrink: 0,
  },
  drawerPaper: {
    width: drawerWidth,
  },
  content: {
    flexGrow: 1,
    padding: theme.spacing(3),
  },
  toolbar: theme.mixins.toolbar,
});

class ToDo extends React.Component {

  async componentDidMount() {
    if (this.props.userProfile) {
      const { access_token, given_name, family_name, email, userId } = this.props.userProfile;
      const data = { id: userId, firstName: given_name, lastName: family_name, email };
      this.props.dispatch(GetorCreateUser(access_token, data));
    }
  }

  handleNewProject = () => {
    this.props.dispatch(OpenProjectDialog());
  }

  handleEditProjects = () => {
    this.props.dispatch(OpenEditProjectsDialog());
  }

  render() {
    const { classes } = this.props;

    return (
      <React.Fragment>
        <Drawer
          className={classes.drawer}
          variant="permanent"
          classes={{
            paper: classes.drawerPaper,
          }}
          PaperProps={{ style: { position: 'absolute' } }}
          BackdropProps={{ style: { position: 'absolute' } }}
          ModalProps={{
            container: document.getElementById('drawer-container'),
            style: { position: 'absolute' }
          }}
        >
          <div className={classes.toolbar} />
          <List>
            {['Inbox', 'Today', 'Next 7 days'].map((text, index) => (
              <ListItem button key={text}>
                <ListItemText primary={text} />
              </ListItem>
            ))}
          </List>
          <ExpansionPanel>
            <ExpansionPanelSummary
              expandIcon={<ExpandMoreIcon />}
              aria-controls="panel1a-content"
              id="panel1a-header"
            >
              <Typography>Projects</Typography>
            </ExpansionPanelSummary>
            <ExpansionPanelDetails>
              <List>
              {this.props.projects.map((project, index) => (
              <ListItem button key={project.id}>
                <ListItemText primary={project.name} />
              </ListItem>
            ))}
              </List>
            </ExpansionPanelDetails>
            <ExpansionPanelActions>
              <Button color="primary" variant="contained" onClick={this.handleNewProject}>Add</Button>
              <Button variant="contained" onClick={this.handleEditProjects} >Edit</Button>
            </ExpansionPanelActions>
          </ExpansionPanel>
          <ExpansionPanel>
            <ExpansionPanelSummary
              expandIcon={<ExpandMoreIcon />}
              aria-controls="panel1b-content"
              id="panel1b-header"
            >
              <Typography>Labels</Typography>
            </ExpansionPanelSummary>
            <ExpansionPanelDetails>
              <List>
              {['Label 1', 'Label 2', 'Label 3'].map((text, index) => (
              <ListItem button key={text}>
                <ListItemText primary={text} />
              </ListItem>
            ))}
              </List>
            </ExpansionPanelDetails>
            <ExpansionPanelActions>
              <Button variant="contained" color="primary">Add</Button>
              <Button variant="contained">Edit</Button>
            </ExpansionPanelActions>
          </ExpansionPanel>
        </Drawer>
        <main className={classes.content}>
          <div className={classes.toolbar} />
          <Typography paragraph>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt
            ut labore et dolore magna aliqua. Rhoncus dolor purus non enim praesent elementum
            facilisis leo vel. Risus at ultrices mi tempus imperdiet. Semper risus in hendrerit
            gravida rutrum quisque non tellus. Convallis convallis tellus id interdum velit laoreet id
            donec ultrices. Odio morbi quis commodo odio aenean sed adipiscing. Amet nisl suscipit
            adipiscing bibendum est ultricies integer quis. Cursus euismod quis viverra nibh cras.
            Metus vulputate eu scelerisque felis imperdiet proin fermentum leo. Mauris commodo quis
            imperdiet massa tincidunt. Cras tincidunt lobortis feugiat vivamus at augue. At augue eget
            arcu dictum varius duis at consectetur lorem. Velit sed ullamcorper morbi tincidunt. Lorem
            donec massa sapien faucibus et molestie ac.
        </Typography>
          <Typography paragraph>
            Consequat mauris nunc congue nisi vitae suscipit. Fringilla est ullamcorper eget nulla
            facilisi etiam dignissim diam. Pulvinar elementum integer enim neque volutpat ac
            tincidunt. Ornare suspendisse sed nisi lacus sed viverra tellus. Purus sit amet volutpat
            consequat mauris. Elementum eu facilisis sed odio morbi. Euismod lacinia at quis risus sed
            vulputate odio. Morbi tincidunt ornare massa eget egestas purus viverra accumsan in. In
            hendrerit gravida rutrum quisque non tellus orci ac. Pellentesque nec nam aliquam sem et
            tortor. Habitant morbi tristique senectus et. Adipiscing elit duis tristique sollicitudin
            nibh sit. Ornare aenean euismod elementum nisi quis eleifend. Commodo viverra maecenas
            accumsan lacus vel facilisis. Nulla posuere sollicitudin aliquam ultrices sagittis orci a.
        </Typography>
        </main>
        <NewProjectDialog />
        <EditProjectsDialog />
      </React.Fragment>
    );
  }
}

function mapStateToProps(state) {
  if (!state.userProfile) {
    return { userProfile: null, snackBar: null, projects: {} };
  }
  var projectData = [];
  if( state.projects && state.projects.Data ){
    projectData = Object.values( state.projects.Data );
  }

  return { userProfile: state.userProfile, snackBar: state.snackBar, projects: projectData };
}

function mapDispatchToProps(dispatch) {
  return {
    dispatch
  };
}

export default connect(mapStateToProps, mapDispatchToProps)((withStyles(useStyles)(withSnackbar(ToDo)))); 