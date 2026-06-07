import { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { getRequestById, updateRequestStatus, deleteRequest } from '../api/serviceRequestApi';
import { ServiceRequest } from '../types';

export default function RequestDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [request, setRequest] = useState<ServiceRequest | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchRequest = async () => {
      try {
        const result = await getRequestById(Number(id));
        if (result.success) {
          setRequest(result.data);
        } else {
          setError(result.message || 'Talep bulunamadı.');
        }
      } catch (err) {
        setError('Sunucu ile iletişim kurarken bir hata oluştu.');
      } finally {
        setLoading(false);
      }
    };
    
    if (id) {
      fetchRequest();
    }
  }, [id]);

  const getTranslatedStatus = (status: string) => {
    switch (status) {
      case 'New': return 'Yeni';
      case 'In Progress': return 'İşlemde';
      case 'Completed': return 'Tamamlandı';
      case 'Cancelled': return 'İptal Edildi';
      default: return status;
    }
  };

  const handleStatusChange = async (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newStatus = e.target.value;
    const translatedStatus = getTranslatedStatus(newStatus);
    
    if (window.confirm(`Bu işlemi ${translatedStatus} olarak işaretlemek istediğinize emin misiniz?`)) {
      try {
        const result = await updateRequestStatus(Number(id), newStatus);
        if (result.success) {
          navigate('/');
        } else {
          setError(result.message || 'Durum güncellenemedi.');
        }
      } catch (err) {
        setError('Durum güncellenirken sunucu hatası oluştu.');
      }
    }
  };

  const handleDelete = async () => {
    if (window.confirm('Bu talebi silmek istediğinize emin misiniz?')) {
      try {
        const result = await deleteRequest(Number(id));
        if (result.success) {
          navigate('/');
        } else {
          setError(result.message || 'Talep silinemedi.');
        }
      } catch (err) {
        setError('Talep silinirken sunucu hatası oluştu.');
      }
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center py-20">
        <div className="animate-spin rounded-full h-10 w-10 border-b-2 border-primary-600"></div>
      </div>
    );
  }

  if (error || !request) {
    return (
      <div className="max-w-3xl mx-auto mt-8 p-6 bg-red-50 text-red-600 rounded-xl border border-red-100">
        <p>{error || 'Talep bulunamadı.'}</p>
        <Link to="/" className="mt-4 inline-block text-primary-600 hover:underline">← Listeye Dön</Link>
      </div>
    );
  }

  return (
    <div className="max-w-3xl mx-auto mt-8 animate-in fade-in slide-in-from-bottom-4 duration-500">
      <div className="bg-white rounded-xl shadow-md border border-slate-100 overflow-hidden">
        <div className="bg-slate-50 px-6 py-4 border-b border-slate-100 flex items-center justify-between">
          <h2 className="text-xl font-bold text-slate-800">Talep Detayları (#{request.id})</h2>
          <Link to="/" className="text-slate-500 hover:text-slate-700 text-sm font-medium">
            ← Listeye Dön
          </Link>
        </div>
        
        <div className="p-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
            <div className="space-y-1">
              <p className="text-sm font-medium text-slate-500">Müşteri Adı</p>
              <p className="text-lg font-semibold text-slate-900">{request.customerName}</p>
            </div>
            <div className="space-y-1">
              <p className="text-sm font-medium text-slate-500">Cihaz Adı</p>
              <p className="text-lg font-semibold text-slate-900">{request.deviceName}</p>
            </div>
            <div className="space-y-1">
              <p className="text-sm font-medium text-slate-500">Oluşturulma Tarihi</p>
              <p className="text-base text-slate-800">
                {new Date(request.createdDate).toLocaleString('tr-TR')}
              </p>
            </div>
            <div className="space-y-1">
              <p className="text-sm font-medium text-slate-500">Mevcut Durum</p>
              <div className="inline-flex mt-1">
                <span className={`text-sm rounded-full px-3 py-1 font-medium ${
                  request.status === 'New' ? 'bg-blue-100 text-blue-800' :
                  request.status === 'In Progress' ? 'bg-amber-100 text-amber-800' :
                  request.status === 'Completed' ? 'bg-emerald-100 text-emerald-800' :
                  'bg-slate-100 text-slate-800'
                }`}>
                  {getTranslatedStatus(request.status)}
                </span>
              </div>
            </div>
          </div>
          
          <div className="mb-8">
            <p className="text-sm font-medium text-slate-500 mb-2">Açıklama</p>
            <div className="bg-slate-50 p-4 rounded-lg border border-slate-100 text-slate-700 whitespace-pre-wrap">
              {request.description}
            </div>
          </div>

          <div className="pt-6 border-t border-slate-100 flex flex-col sm:flex-row items-start sm:items-end justify-between gap-4">
            <div className="w-full sm:w-1/2">
              <label htmlFor="status" className="block text-sm font-medium text-slate-700 mb-2">
                Durumu Güncelle
              </label>
              <select
                id="status"
                value={request.status}
                onChange={handleStatusChange}
                className="w-full px-4 py-2 border border-slate-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary-500 bg-white shadow-sm transition-all"
              >
                <option value="New">Yeni</option>
                <option value="In Progress">İşlemde</option>
                <option value="Completed">Tamamlandı</option>
                <option value="Cancelled">İptal Edildi</option>
              </select>
            </div>
            
            <button
              onClick={handleDelete}
              className="px-4 py-2 bg-red-50 text-red-600 hover:bg-red-500 hover:text-white rounded-lg transition-colors font-medium border border-red-100 shadow-sm w-full sm:w-auto"
            >
              Talebi Sil
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
