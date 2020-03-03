import React from "react";
import { connect } from "react-redux";
import { GetorCreateUser, LoadLabelsAndProjectsForUser } from '../actions';
import { withStyles } from '@material-ui/core/styles';
import Drawer from '@material-ui/core/Drawer';
import List from '@material-ui/core/List';
import Typography from '@material-ui/core/Typography';
import Divider from '@material-ui/core/Divider';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';

const drawerWidth = 240;

const useStyles = theme => ({
  drawer: {
    width: drawerWidth,
    flexShrink: 0,
    height: '90vh'
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
    if (this.props.user) {

      //There has got to be a better way to do this - I would maybe attach to the reducer and snip off the profile part that I want to make it easier. 
      const { access_token } = this.props.user;
      const { given_name, family_name, email, sub } = this.props.user.profile;
      const data = { id: sub, firstName: given_name, lastName: family_name, email };

      this.props.dispatch(GetorCreateUser(access_token, data));
      this.props.dispatch(LoadLabelsAndProjectsForUser(access_token));

    }
  }



  render() {
    const { classes } = this.props;
    return (
      <div id="drawer-container" style={{ position: 'relative' }}>
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
          <Divider />
          <List>
            {['Projects'].map((text, index) => (
              <ListItem button key={text}>
                <ListItemText primary={text} />
              </ListItem>
            ))}
          </List>
          <List>
            {['Labels'].map((text, index) => (
              <ListItem button key={text}>
                <ListItemText primary={text} />
              </ListItem>
            ))}
          </List>
        </Drawer>
        <main className={classes.content}>
          <div className={classes.toolbar} />
          <Typography paragraph>
            Figure out how to position this div correctly.
          </Typography>
        </main>
      </div>
    );
  }
}

function mapStateToProps(state) {
  const user = state.oidc.user;
  if (!user) {
    return { user: null };
  }
  return { user: user };
}

function mapDispatchToProps(dispatch) {
  return {
    dispatch
  };
}

export default connect(mapStateToProps, mapDispatchToProps)((withStyles(useStyles)(ToDo)));