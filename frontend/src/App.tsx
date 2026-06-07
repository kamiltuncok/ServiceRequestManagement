import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import RequestList from './components/RequestList';
import RequestForm from './components/RequestForm';
import RequestDetail from './components/RequestDetail';

function App() {
  return (
    <Router>
      <div className="min-h-screen bg-gray-50 text-slate-800">
        <nav className="bg-primary-600 text-white shadow-md">
          <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div className="flex justify-between h-16">
              <div className="flex">
                <div className="flex-shrink-0 flex items-center text-xl font-bold tracking-tight">
                  <Link to="/">Servis Talep Yönetimi</Link>
                </div>
                <div className="hidden sm:ml-6 sm:flex sm:space-x-8">
                  <Link
                    to="/"
                    className="border-transparent text-white hover:border-primary-300 hover:text-primary-100 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium transition-colors"
                  >
                    Kontrol Paneli
                  </Link>
                  <Link
                    to="/create"
                    className="border-transparent text-white hover:border-primary-300 hover:text-primary-100 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium transition-colors"
                  >
                    Yeni Talep
                  </Link>
                </div>
              </div>
            </div>
          </div>
        </nav>

        <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
          <Routes>
            <Route path="/" element={<RequestList />} />
            <Route path="/create" element={<RequestForm />} />
            <Route path="/requests/:id" element={<RequestDetail />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
