import React from 'react'
import { AppBar, Toolbar, IconButton, Typography, Menu, MenuItem, ListItemIcon, ListItemText } from '@material-ui/core'
import { withStyles } from '@material-ui/core/styles';
import LoginLogout from './LoginLogout';
import { connect } from 'react-redux';
import SettingsIcon from '@material-ui/icons/Settings';
import AddIcon from '@material-ui/icons/Add';
import AssignmentTurnedInIcon from '@material-ui/icons/AssignmentTurnedIn';
import AccountTreeIcon from '@material-ui/icons/AccountTree';
import LabelIcon from '@material-ui/icons/Label';

const useStyles = theme => ({
  root: {
    flexGrow: 1,
  },
  appBar: {
    zIndex: theme.zIndex.drawer + 1,
  },
  menuButton: {
    marginRight: theme.spacing(2),
  },
  title: {
    flexGrow: 1,
  },
});

const StyledMenu = withStyles({
  paper: {
    border: '1px solid #d3d4d5',
  },
})(props => (
  <Menu
    elevation={0}
    getContentAnchorEl={null}
    anchorOrigin={{
      vertical: 'bottom',
      horizontal: 'center',
    }}
    transformOrigin={{
      vertical: 'top',
      horizontal: 'center',
    }}
    {...props}
  />
));

const StyledMenuItem = withStyles(theme => ({
  root: {
    '&:focus': {
      backgroundColor: theme.palette.primary.main,
      '& .MuiListItemIcon-root, & .MuiListItemText-primary': {
        color: theme.palette.common.white,
      },
    },
  },
}))(MenuItem);

class Header extends React.Component {

  constructor(props){
    super(props);
    this.state = { anchorEl:null };
  }

  handleMenu = event => {
    this.setState({anchorEl: event.currentTarget});
  };

  handleClose = () => {
    this.setState({anchorEl: null})
  };

  render() {
    const { classes } = this.props;
    return (
      <AppBar position="fixed" className={classes.appBar} >
        <Toolbar>
          <Typography variant="h6" className={classes.title}>
            To Do List
          </Typography>
          <IconButton 
            aria-label="Add Task, Project or Label" 
            aria-controls="menu-add"
            aria-haspopup="true"
            onClick={this.handleMenu}
            color="inherit"
          >
            <AddIcon />
          </IconButton>

          <StyledMenu
            id="menu-add"
            anchorEl={this.state.anchorEl}
            keepMounted
            open={this.state.anchorEl != null}
            onClose={this.handleClose}
          >
            <StyledMenuItem onClick={this.handleClose}>
              <ListItemIcon>
                <AssignmentTurnedInIcon fontSize="small" />
              </ListItemIcon>
              <ListItemText primary="Add To Do" />
            </StyledMenuItem>
            <StyledMenuItem onClick={this.handleClose}>
              <ListItemIcon>
                <AccountTreeIcon fontSize="small" />
              </ListItemIcon>
              <ListItemText primary="Add Project" />
            </StyledMenuItem>
            <StyledMenuItem onClick={this.handleClose}>
              <ListItemIcon>
                <LabelIcon fontSize="small" />
              </ListItemIcon>
              <ListItemText primary="Add Label" />
            </StyledMenuItem>
          </StyledMenu>

          <IconButton aria-label="settings" color="inherit">
            <SettingsIcon />
          </IconButton>
          <LoginLogout />
        </Toolbar>
      </AppBar>
    );
  }
}

const mapPropsToState = (state) => {
  const user = state.oidc.user;
  if (!user) {
    return { user: null, userName: null };
  }
  else {
    return { user: user, userName: user.profile.name };
  }
};

export default connect(mapPropsToState)(withStyles(useStyles)(Header));