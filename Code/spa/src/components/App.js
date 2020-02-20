import React from 'react'
import Header from './Header'
import Typography from '@material-ui/core/Typography';
import Container from '@material-ui/core/Container';
import { HashRouter, Route } from 'react-router-dom'
// https://www.gistia.com/react-authentication-security-private-routes/ for learning how to secure routes.
const Login = () => {
    return <div>Please Login to Begin</div>
}

const App = ()=> {
    return( 
        <React.Fragment>
            <Container maxWidth="lg">
                <Typography component="div" style={{ backgroundColor: '#d3d3d3', height: '100vh' }}>
                    <Header />
                    <HashRouter>
                        <dib>
                            <Route path="/" exact component={Login} />
                        </dib>
                        
                    </HashRouter>
                </Typography>
            </Container>
        </React.Fragment>
    );
};

export default App