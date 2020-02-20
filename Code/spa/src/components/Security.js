import React from 'react'
import { Button } from '@material-ui/core'
import OpenId from 'oidc-client'

class Security extends React.Component{
    state = { isSignedIn: null };

    componentDidMount(){
        var config = {
            authority: "http://localhost:5000",
            client_id: "js",
            redirect_uri: "http://localhost:3000/callback.html",
            response_type: "code",
            scope: "openid profile api1",
            post_logout_redirect_uri: "http://localhost:3000/index.html",
        };

        var mgr = new OpenId.UserManager(config);
        //mgr.events.   <== Need a way to subscribe to when a user is logged in or out - update redux state
        // This looks very promissing:  https://github.com/maxmantz/redux-oidc
        mgr.getUser().then((user) => {
            if( user ){
                this.setState( { isSignedIn: true } );
                
            }
            else{
                this.setState( { isSignedIn: false } );
            }
        });
    }

    renderAuthButton() {
        if( this.state.isSignedIn === null){
            return <div>Loading...</div>
        }
        else if (this.state.isSignedIn){
            return <Button color="inherit">Logout</Button>
        }
        else{
            return <Button color="inherit">Login</Button>
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

export default Security