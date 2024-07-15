import {
  Box,
  Modal,
  TextField,
  Typography,
  Container,
  Button,
  Snackbar,
  Alert,
} from "@mui/material";
import axios from "axios";
import { useState } from "react";

interface PromoModalProps {
  open: boolean;
  onClose: () => void;
}

export const PromoModal = ({ open, onClose }: PromoModalProps) => {
  const [promoText, setPromoText] = useState<string>("Ваш промокод");
  const [promoType, setPromoType] = useState<string>("Fixed");
  const [discount, setDiscount] = useState<number>(0);
  const [expiration, setExpiration] = useState<number>(1);
  const [succeed, setSucceed] = useState<boolean>(false);

  const handleSavePromo = async () => {
    try {
      const promoTypeValue = promoType === "Fixed" ? 0 : 1;
      const response = await axios.post(`/admin/add-promocode`, {
        Code: promoText,
        PromocodeType: promoTypeValue,
        Discount: discount,
        ExpirationInHours: expiration,
      });

      if (response.status === 200) {
        setSucceed(true);
        onClose();
      }
    } catch (error) {
      console.error(error);
    }
  };

  const handleCloseSnack = () => {
    setSucceed(false);
  };

  return (
    <Modal
      open={open}
      onClose={onClose}
      aria-labelledby="modal-title"
      aria-describedby="modal-description">
      <Container
        sx={{
          bgcolor: "#f0f0f0",
          borderRadius: "10px",
          width: "500px",
        }}>
        <Typography variant="h4" sx={{ fontWeight: "500", color: "fff" }}>
          Создание промокода
        </Typography>
        <Box>
          <Typography>Введите текст промокода: </Typography>
          <TextField
            id="filled-basic"
            label="Промокод"
            variant="filled"
            sx={{ width: "100%" }}
            value={promoText}
            onChange={(e) => setPromoText(e.target.value)}
          />
          <Typography>Выберите тип промокода: </Typography>
          <Typography sx={{ background: "#e3e3e3" }}>{promoType}</Typography>
          <Button
            sx={{ margin: 1 }}
            variant="contained"
            onClick={() => setPromoType("Fixed")}>
            Абсолютная скидка
          </Button>
          <Button
            sx={{ margin: 1 }}
            variant="contained"
            onClick={() => setPromoType("Percent")}>
            Скидка в процентах
          </Button>
          <Typography>Размер скидки:</Typography>
          <TextField
            id="filled-basic"
            label="Скидка"
            variant="filled"
            sx={{ width: "100%" }}
            value={discount}
            onChange={(e) => setDiscount(Number(e.target.value))}
          />
          <Typography>Сколько часов будет действовать:</Typography>
          <TextField
            id="filled-basic"
            label="Длительность"
            variant="filled"
            sx={{ width: "100%" }}
            value={expiration}
            onChange={(e) => setExpiration(Number(e.target.value))}
          />
        </Box>
        <Button
          sx={{ margin: 1 }}
          variant="contained"
          onClick={() => {
            setSucceed(false), onClose();
          }}>
          Отмена
        </Button>
        <Button
          sx={{ margin: 1 }}
          variant="contained"
          onClick={handleSavePromo}>
          Сохранить
        </Button>
        <Snackbar
          open={succeed}
          autoHideDuration={5000}
          onClose={handleCloseSnack}>
          <Alert
            onClose={handleCloseSnack}
            severity="success"
            variant="filled"
            sx={{ width: "100%" }}>
            Промокод успешно добавлен
          </Alert>
        </Snackbar>
      </Container>
    </Modal>
  );
};
