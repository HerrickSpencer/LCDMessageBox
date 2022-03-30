var express = require('express');
var router = express.Router();

router.post('/msgHandle', function (req, res) {
    console.log(req.body);
    res.render('index', { title: 'Cigar Box Message handled', msg: req.body.msg });
  });
  
module.exports = router;
