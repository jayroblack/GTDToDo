import React from 'react'
import Header from './Header'
import Typography from '@material-ui/core/Typography';
import Container from '@material-ui/core/Container';

const App = ()=> {
    return( 
        <React.Fragment>
            <Header />
            <Container maxWidth="lg">
                <Typography component="div" style={{ height: '100vh' }} />
            </Container>
        </React.Fragment>
    );
};

export default App