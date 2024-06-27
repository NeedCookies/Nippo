import React, { useState } from 'react';
import { Modal, Box, Button, TextField, Alert } from '@mui/material';
import axios, { AxiosError } from 'axios';

interface PointModalProps {
    open: boolean;
    onClose: () => void;
}

export const PointsModal = ({open, onClose} : PointModalProps) => {
    const [userId, setUserId] = useState<string>('');
    const [points, setPoints] = useState<number>(1);
    const [response, setResponse] = useState<{ type: 'success' | 'error', message: string } | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setIsLoading(true);

        if (!userId) {
            console.log(points);
            setResponse({ type: 'error', message: 'Пожалуйста, заполните все поля.' });
            return;
        }

        try{
            const response = await axios.post("/admin/give-points",
                { 
                    userId: userId,
                    points: points
                }
            );

            if (response.status == 200) {
                setResponse({ type: 'success', message: 'Поинты были успешно добавлены'})
            }
        }
        catch(error){
            if (error instanceof AxiosError) {
                if (error.response && error.response.status == 400) {
                    let errorText = error.response.data[0];
                    setResponse({ type: 'error', message: errorText})
                } else{
                    setResponse({ type: 'error', message: `Ошибка на стороне клиента: ${error}`})
                }
            }
            else {
                // Обработка других ошибок (не AxiosError)
                console.error('Ошибка:', (error as Error).message);
            }
        }
        
        setIsLoading(false);
    };

    return (
        <div>
            <Modal
                open={open}
                onClose={onClose}
                aria-labelledby="modal-title"
                aria-describedby="modal-description"
            >
                <Box sx={modalStyle}>
                    <h2 id="modal-title">Добавить поинты пользователю</h2>
                    <form onSubmit={handleSubmit}>
                        <TextField
                            fullWidth
                            margin="normal"
                            label="Id пользователя"
                            variant="outlined"
                            value={userId}
                            onChange={(e) => setUserId(e.target.value)}
                        />
                        <TextField
                            fullWidth
                            margin="normal"
                            label="Количество поинтов"
                            variant="outlined"
                            value={points}
                            type="number"
                            onChange={(e) => {
                                const value = parseInt(e.target.value);
                                setPoints(value);
                            }}
                        />
                        {response && (
                            <Alert severity={response.type} sx={{ margin: '20px 0' }}>
                                {response.message}
                            </Alert>
                        )}
                        <div style={{ textAlign: "center" }}>
                            <Button variant="contained" color="primary" type="submit" disabled={isLoading}>
                                Сохранить
                            </Button>
                        </div>
                    </form>
                </Box>
            </Modal>
        </div>
    );
};

const modalStyle = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400,
    bgcolor: 'background.paper',
    boxShadow: 24,
    p: 4,
    borderRadius: '8px',
};