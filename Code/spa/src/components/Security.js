import React from 'react'
import { connect } from 'react-redux'
import { Button } from '@material-ui/core'
import userManager from "../userManager";

class Security extends React.Component{

    onLoginButtonClick = (event) => {
        event.preventDefault();
        userManager.signinRedirect();
    }

    onLogoutButtonClick = (event) => {
        event.preventDefault();
        userManager.signoutRedirect();
    }

    renderAuthButton() {
        if (this.props.user){
            return <Button color="inherit">Logout</Button>
        }
        else{
            return <Button onClick={this.onLoginButtonClick} color="inherit">Login</Button>
        }
    }

    render(){
        return(
        <React.Fragment>
            {this.renderAuthButton()}
        </React.Fragment>
        );
    }
}

const mapPropsToState = (state) => {
    return {
        user : state.oidc.user
    }
};

export default connect(mapPropsToState)(Security);