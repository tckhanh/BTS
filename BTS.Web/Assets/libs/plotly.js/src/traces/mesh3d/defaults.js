/**
* Copyright 2012-2018, Plotly, Inc.
* All rights reserved.
*
* This source code is licensed under the MIT license found in the
* LICENSE file in the root directory of this source tree.
*/


'use strict';

var Registry = require('../../registry');
var Lib = require('../../lib');
var colorscaleDefaults = require('../../components/colorscale/defaults');
var attributes = require('./attributes');

module.exports = function supplyDefaults(traceIn, traceOut, defaultColor, layout) {
    function coerce(attr, dflt) {
        return Lib.coerce(traceIn, traceOut, attributes, attr, dflt);
    }

    // read in face/vertex properties
    function readComponents(array) {
        var ret = array.map(function(attr) {
            var result = coerce(attr);

            if(result && Lib.isArrayOrTypedArray(result)) return result;
            return null;
        });

        return ret.every(function(x) {
            return x && x.length === ret[0].length;
        }) && ret;
    }

    var coords = readComponents(['x', 'y', 'z']);
    var indices = readComponents(['i', 'j', 'k']);

    if(!coords) {
        traceOut.visible = false;
        return;
    }

    if(indices) {
        // otherwise, convert all face indices to ints
        indices.forEach(function(index) {
            for(var i = 0; i < index.length; ++i) index[i] |= 0;
        });
    }

    var handleCalendarDefaults = Registry.getComponentMethod('calendars', 'handleTraceDefaults');
    handleCalendarDefaults(traceIn, traceOut, ['x', 'y', 'z'], layout);

    // Coerce remaining properties
    [
        'lighting.ambient',
        'lighting.diffuse',
        'lighting.specular',
        'lighting.roughness',
        'lighting.fresnel',
        'lighting.vertexnormalsepsilon',
        'lighting.facenormalsepsilon',
        'lightposition.x',
        'lightposition.y',
        'lightposition.z',
        'contour.show',
        'contour.color',
        'contour.width',
        'colorscale',
        'reversescale',
        'flatshading',
        'alphahull',
        'delaunayaxis',
        'opacity'
    ].forEach(function(x) { coerce(x); });

    if('intensity' in traceIn) {
        coerce('intensity');
        colorscaleDefaults(traceIn, traceOut, layout, coerce, {prefix: '', cLetter: 'c'});
    } else {
        traceOut.showscale = false;

        if('facecolor' in traceIn) coerce('facecolor');
        else if('vertexcolor' in traceIn) coerce('vertexcolor');
        else coerce('color', defaultColor);
    }

    coerce('text');
};