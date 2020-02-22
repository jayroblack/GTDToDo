import React from 'react'
import { Button } from '@material-ui/core'

class Security extends React.Component{
    state = { isSignedIn: null };

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